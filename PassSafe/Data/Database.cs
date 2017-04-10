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

namespace PassSafe.Data
{
    class Database
    {
        string fileName;
        SQLiteConnection conn;
        string connString;
        bool databaseExists = false;

        public Database()
        {
            this.fileName = Directory.GetCurrentDirectory() + @"\database.sqlite3";
            this.connString = String.Format("Data Source={0};Version=3;", this.fileName);

            if (!File.Exists(this.fileName))
            {
                if (!this.CreateDatabase())
                    return;
                else
                    this.databaseExists = true;
            } else
            {
                this.databaseExists = true;
            }

            this.conn = new SQLiteConnection(this.connString);
        }

        private bool CreateDatabase()
        {
            try
            {
                SQLiteConnection.CreateFile(this.fileName);
                using (var sqlite3 = new SQLiteConnection(this.connString))
                {
                    sqlite3.Open();
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("CREATE TABLE 'Services'( `id` INTEGER PRIMARY KEY AUTOINCREMENT, `serviceName` TEXT NOT NULL, `email` TEXT NOT NULL, `loginName` TEXT NOT NULL, `description` TEXT NOT NULL, `password` TEXT NOT NULL, `hash` TEXT NOT NULL, `lastUpdated` INTEGER NOT NULL)");
                }
                return true;
            } catch (Exception e)
            {
                Core.PrintDebug(e.Message);
                return false;
            }
        }

        public List<Service> GetServices()
        {
            string sql = @"select * from Services;";
            DataTable dataTable = this.Select(sql);
            if (dataTable.Rows.Count > 0)
            {
                try
                {
                    Core.PrintDebug("Reading Services from database...");
                    List<Service> thisList = new List<Service>();
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Service service = new Service();
                        service.Id = Convert.ToInt32(dr["id"]);
                        service.ServiceName = dr.Field<string>("serviceName");
                        service.Email = dr.Field<string>("email");
                        service.UserName = dr.Field<string>("loginName");
                        service.Description = dr.Field<string>("description");
                        service.HashedPassword = dr.Field<string>("password");
                        service.PasswordHash = dr.Field<string>("hash");
                        thisList.Add(service);
                    }

                    return thisList;
                } catch (Exception e)
                {
                    Core.PrintDebug(e.Message);
                    Core.PrintDebug(e.StackTrace);
                }
            }

            return new List<Service>
            {
                new Service()
                {
                    Id = -1,
                    ServiceName = "ExampleService",
                    Email = "sam@samdkershaw.com",
                    UserName = "ExampleUsername",
                    Description = "This is an example description.",
                    HashedPassword = "password",
                    PasswordHash = "lol"
                }
            };
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
            } catch (Exception e)
            {
                Core.PrintDebug(e.Message);
            }
            return dt;
        }

        public bool DeleteService(int rowId)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                SQLiteTransaction trans = conn.BeginTransaction();
                cmd.Connection = this.conn;
                cmd.CommandText = "delete from services where id = @id";
                cmd.Parameters.AddWithValue("@id", rowId.ToString());
                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            } catch (Exception e)
            {
                Core.PrintDebug(e.Message);
                return false;
            }
        }
    }
}
