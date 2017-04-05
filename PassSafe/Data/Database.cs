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

        public Database()
        {
            this.fileName = Directory.GetCurrentDirectory() + @"\database.sqlite3";
            this.conn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", this.fileName));
        }

        public List<Service> GetServices()
        {
            string sql = @"select * from Services;";
            DataTable dataTable = this.Select(sql);

            List<Service> list = dataTable.Rows.OfType<DataRow>().Select(
                dataRow => new Service
                {
                    ServiceName = dataRow.Field<string>("ServiceName"),
                    UserName = dataRow.Field<string>("UserName"),
                    Email = dataRow.Field<string>("Email"),
                    Password = dataRow.Field<SecureString>("Password")
                }).ToList();

            return list;
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
                SQLiteCommand cmd = new SQLiteCommand(this.conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close(); this.conn.Close();
                return dt;
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
