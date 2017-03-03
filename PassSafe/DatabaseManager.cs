using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Project
{
    sealed class DatabaseManager
    {
        private static readonly DatabaseManager instance = new DatabaseManager();

        static DatabaseManager()
        {
        }

        private DatabaseManager()
        {

        }

        public static DatabaseManager GetDatabaseManager
        {
            get
            {
                return instance;
            }
        }

        bool createDatabase()
        {
            if (File.Exists("database.db3"))
            {
                return true;
            } else
            {
                try
                {
                    return true;
                } catch (Exception e)
                {
                    Core.PrintErrorToFile(e.Message);
                    return false;
                }
            }
        }
    }
}
