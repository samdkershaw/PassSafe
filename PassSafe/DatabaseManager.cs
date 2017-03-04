using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace PassSafe
{
    sealed class DatabaseManager
    {
        private static readonly DatabaseManager instance = new DatabaseManager();
        private string databaseLocation;

        // Database Variables


        static DatabaseManager()
        {
        }

        private DatabaseManager()
        {
            databaseLocation = Core.GetAppDirectory() + @"\res\database.db3";
        }

        public static DatabaseManager GetDatabaseManager
        {
            get
            {
                return instance;
            }
        }

        bool IsDatabaseCreated()
        {
            if (File.Exists(databaseLocation))
                return true;
            else
                return CreateDatabase();
        }

        private bool CreateDatabase()
        {
            try
            {
                SQLiteConnection.CreateFile(databaseLocation);
                SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=\res\database.sqlite;Version=3;");
                m_dbConnection.Open();

                string sql = "create table user_info (name varchar(40), email varchar(50)";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                return true;

            } catch (Exception e)
            {
                Core.PrintErrorToFile(e.Message);
                return false;
            }
        }

        private void RunSql(string sql)
        {
            //if ()
        }
    }
}
