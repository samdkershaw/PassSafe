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
            this.conn = new SQLiteConnection("Data Source=" + fileName + ";Version=3;");
        }

        public List<Models.Service> GetServices()
        {
            string sql = @"select * from Services;";
            DataSet ds = Select(sql);

            List<Service> list = ds.Tables[0].AsEnumerable().Select(
                dataRow => new Models.Service {
                    ServiceName = dataRow.Field<string>("ServiceName"),
                    UserName = dataRow.Field<string>("UserName"),
                    Email = dataRow.Field<string>("Email"),
                    Password = dataRow.Field<SecureString>("Password")
                }).ToList();

            return list;
        }

        //public UserInfo

        public DataSet Select(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                this.conn.Open();
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
                da.Fill(ds);
                this.conn.Close();
                return ds;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
