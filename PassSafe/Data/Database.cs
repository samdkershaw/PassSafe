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
        SQLiteConnection conn;
        string connString;

        public Database()
        {
            this.fileName = Directory.GetCurrentDirectory() + @"\database.sqlite3";
            this.connString = String.Format("Data Source={0};Version=3;", this.fileName);

            FileInfo dbInfo = new FileInfo(this.fileName);

            //if (!dbInfo.Exists || dbInfo.Length == 0)
            //{
            //    if (!CreateDatabase())
            //        return;
            //}

            this.conn = new SQLiteConnection(this.connString);
        }

        public bool DoesDatabaseExist()
        {
            FileInfo dbInfo = new FileInfo(this.fileName);
            return dbInfo.Exists;
        }

        public bool IsReturningUser()
        {
            string sql = "SELECT Count(*) FROM UserInfo";
            using (this.conn)
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(this.conn);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text; 
                int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                return userCount == 1;
            }
        }

        public bool CreateDatabase()
        {
            bool success = false;
            try
            {
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

        public UserInfo GetUserInfo()
        {
            UserInfo info = UserInfo.Instance;
            string sql = @"select * from User;";
            DataTable dataTable = this.Select(sql);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            return info;
        }

        public DataTable Select(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                this.conn.Open();
                //SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
                SQLiteCommand cmd = new SQLiteCommand(sql, this.conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                this.conn.Close();
                return dt;
            } catch (SQLiteException e)
            {
                Core.PrintDebug(e.Message);
                Core.PrintDebug(e.StackTrace);
            }
            return dt;
        }

        public bool DeleteService(int rowId)
        {
            bool success = false;
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                this.conn.Open();
                SQLiteTransaction trans = conn.BeginTransaction();
                cmd.Connection = this.conn;
                cmd.CommandText = "delete from services where id = @id";
                cmd.Parameters.AddWithValue("@id", rowId.ToString());
                cmd.ExecuteNonQuery();
                trans.Commit();
                success = true;
            } catch (Exception e)
            {
                Core.PrintDebug(e.Message);
                Core.PrintDebug(e.StackTrace);
            }
            finally
            {
                this.conn.Close();
            }

            return success;
        }
    }
}
