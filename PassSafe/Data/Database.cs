using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Security;
using PassSafe.Models;
using System.Collections.ObjectModel;

namespace PassSafe.Data
{
    public class Database
    {
        string fileName;
        string connString;

        public Database()
        {
            this.fileName = Directory.GetCurrentDirectory() + @"\database.sqlite3";
            this.connString = String.Format("Data Source={0};Version=3;", this.fileName);

            FileInfo dbInfo = new FileInfo(this.fileName);
        }

        public bool DoesDatabaseExist()
        {
            FileInfo dbInfo = new FileInfo(this.fileName);
            return dbInfo.Exists && this.VerifyTableIntegrity();
        }

        public bool VerifyTableIntegrity()
        {
            string userInfoSql = "SELECT Count(*) FROM UserInfo";
            string servicesSql = "SELECT Count(*) FROM Services";
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(conn);
                    command.CommandText = userInfoSql;
                    command.CommandType = CommandType.Text;
                    int userCount = Convert.ToInt32(command.ExecuteScalar());

                } catch (SQLiteException e)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsReturningUser()
        {
            string sql = "SELECT Count(*) FROM UserInfo";
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    return userCount == 1;
                } catch (SQLiteException e)
                {
                    Core.PrintDebug(e.Message);
                    return false;
                }
            }
        }

        public bool CreateDatabase()
        {
            bool success = false;
            try
            {
                if (File.Exists(this.fileName))
                {
                    try
                    {
                        File.Delete(this.fileName);
                    } catch (IOException e)
                    {
                        Core.PrintDebug(e.Message);
                    }
                }
                SQLiteConnection.CreateFile(this.fileName);
                using (var sqlite3 = new SQLiteConnection(this.connString))
                {
                    sqlite3.Open();
                    string userSql = @"CREATE TABLE 'UserInfo'(
`id` INTEGER PRIMARY KEY AUTOINCREMENT,
`firstName` TEXT NOT NULL,
`lastName` TEXT NOT NULL,
`masterPassword` TEXT NOT NULL,
`masterPasswordHash` TEXT NOT NULL,
`emailAddress` TEXT NOT NULL,
`deviceId` TEXT)";
                    SQLiteCommand userCommand = new SQLiteCommand(userSql, sqlite3);
                    userCommand.ExecuteNonQuery();
                    string servicesSql = @"CREATE TABLE 'Services'(
`id` INTEGER PRIMARY KEY AUTOINCREMENT,
`serviceName` TEXT NOT NULL, 
`email` TEXT NOT NULL, 
`loginName` TEXT NOT NULL, 
`description` TEXT NOT NULL, 
`password` TEXT NOT NULL, 
`hash` TEXT NOT NULL, 
`lastUpdated` NUMERIC NOT NULL,
`website` TEXT NOT NULL)";
                    SQLiteCommand servicesCommand = new SQLiteCommand(servicesSql, sqlite3);
                    servicesCommand.ExecuteNonQuery();
                }
                success = true;
            } catch (Exception e)
            {
                Core.PrintDebug(e.Message);
                File.Delete(this.fileName);
            }
            return success;
        }

        public ObservableCollection<Service> GetServices()
        {
            string sql = @"select * from Services;";
            DataTable dataTable = this.Select(sql);
            if (dataTable.Rows.Count > 0)
            {
                try
                {
                    Core.PrintDebug("Reading Services from database...");
                    ObservableCollection<Service> thisList = new ObservableCollection<Service>();
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Service service = new Service();
                        service.Id = Convert.ToInt32(dr["id"]);
                        service.ServiceName = dr.Field<string>("serviceName");
                        service.Email = dr.Field<string>("email");
                        service.Website = dr.Field<string>("website");
                        service.UserName = dr.Field<string>("loginName");
                        service.Description = dr.Field<string>("description");
                        service.HashedPassword = dr.Field<string>("password");
                        service.PasswordHash = dr.Field<string>("hash");
                        service.LastUpdated = Core.UnixTimestampToDateTime(Convert.ToInt64(dr["lastUpdated"]));
                        thisList.Add(service);
                    }

                    return thisList;
                } catch (Exception e)
                {
                    Core.PrintDebug(e.Message);
                    Core.PrintDebug(e.StackTrace);
                }
            }

            return new ObservableCollection<Service>();
        }

        public bool CreateNewUser(UserInfo userInfo)
        {
            bool success = false;
            try
            {
                using (var sqlite3 = new SQLiteConnection(this.connString))
                {
                    sqlite3.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlite3);
                    command.CommandText = @"INSERT INTO UserInfo (firstName, lastName, masterPassword, masterPasswordHash, emailAddress)
VALUES (@param1, @param2, @param3, @param4, @param5);";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@param1", userInfo.Forename));
                    command.Parameters.Add(new SQLiteParameter("@param2", userInfo.Surname));
                    command.Parameters.Add(new SQLiteParameter("@param3", userInfo.MasterPassword));
                    command.Parameters.Add(new SQLiteParameter("@param4", userInfo.PasswordHash));
                    command.Parameters.Add(new SQLiteParameter("@param5", userInfo.EmailAddress));
                    command.ExecuteNonQuery();
                }
                success = true;
            }
            catch (Exception e)
            {
                Core.PrintDebug(e.Message);
            }
            return success;
        }

        public Dictionary<string, object> GetUserInfo()
        {
            string sql = @"select * from UserInfo;";
            DataTable dataTable = this.Select(sql);
            try
            {
                DataRow dr = dataTable.Rows[0];
                Core.PrintDebug("Reading User Info from database...");
                Dictionary<string, object> holder = new Dictionary<string, object>();
                holder.Add("Id", Convert.ToInt32(dr["id"]));
                holder.Add("Forename", dr.Field<string>("firstName"));
                holder.Add("Surname", dr.Field<string>("lastName"));
                holder.Add("MasterPassword", dr.Field<string>("masterPassword"));
                holder.Add("PasswordHash", dr.Field<string>("masterPasswordHash"));
                holder.Add("EmailAddress", dr.Field<string>("emailAddress"));
                return holder;
            } catch (Exception e)
            {
                Core.PrintDebug(e.Message);
                Core.PrintDebug(e.StackTrace);
            }
            return new Dictionary<string, object>();
        }

        public DataTable Select(string sql)
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open();
                    //SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
                catch (SQLiteException e)
                {
                    Core.PrintDebug(e.Message);
                    Core.PrintDebug(e.StackTrace);
                }
            }
            return dt;
        }

        public bool DeleteService(int rowId)
        {
            bool success = false;
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandText = "delete from services where id = @id";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", rowId.ToString());
                    cmd.ExecuteNonQuery();
                    success = true;
                }
                catch (Exception e)
                {
                    Core.PrintDebug(e.Message);
                    Core.PrintDebug(e.StackTrace);
                }
            }
            return success;
        }
    }
}