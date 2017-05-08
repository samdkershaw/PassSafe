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
        
        /// <summary>
        /// This constructor sets up the connection with the database by locating the database
        /// relative to the application executable. A connection string is then created.
        /// </summary>
        public Database()
        {
            this.fileName = Directory.GetCurrentDirectory() + @"\database.sqlite3";
            this.connString = String.Format("Data Source={0};Version=3;", this.fileName);
        }

        /// <summary>
        /// This method calculates whether or not the database exists, and whether or not the
        /// table structure is as it should be.
        /// </summary>
        /// <returns>Whether the database exists and the table structure is intact</returns>
        public bool DoesDatabaseExist()
        {
            FileInfo dbInfo = new FileInfo(this.fileName); //Get the info about the database.sqlite3 file.
            return dbInfo.Exists && this.VerifyTableIntegrity();
        }


        /// <summary>
        /// This function attempts to retrieve the count in the two tables of the database.
        /// If the tables don't exist, then an exception will be thrown causing 'false'
        /// to be returned. Otherwise, true will be returned.
        /// </summary>
        /// <returns>Returns true if tables are correct, or false if not.</returns>
        public bool VerifyTableIntegrity()
        {
            string userInfoSql = "SELECT Count(*) FROM UserInfo";
            string servicesSql = "SELECT Count(*) FROM Services";
            //using statement ensures that the connection is disposed of
            using (var conn = new SQLiteConnection(this.connString))
            {
                // Catch any exceptions that may occur.
                try
                {
                    conn.Open(); // Opens the connection to the SQLite database.
                    //Create the SQLite command and assign the SQL statement to it.
                    SQLiteCommand command = new SQLiteCommand(conn);
                    command.CommandText = userInfoSql;
                    command.CommandType = CommandType.Text;
                    //Get the number of rows in the UserInfo table back.
                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    SQLiteCommand servicesCommand = new SQLiteCommand(conn);
                    servicesCommand.CommandText = servicesSql;
                    servicesCommand.CommandType = CommandType.Text;
                    int servicesCount = Convert.ToInt32(servicesCommand.ExecuteScalar());
                } catch (SQLiteException e)
                {
                    //An exception will occur if one of the tables doesn't exist.
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This method checks whether or not a row exists in the UserInfo table, which means the user has
        /// already registered before.
        /// </summary>
        /// <returns>true if the user has registered already, false otherwise</returns>
        public bool IsReturningUser()
        {
            string sql = "SELECT Count(*) FROM UserInfo";
            //using statement ensures that the connection is disposed of after use
            using (var conn = new SQLiteConnection(this.connString))
            {
                // Catch any exceptions that occur.
                try
                {
                    conn.Open(); //Open the connection.
                    //Build the SQLite command and assign the SQL statement to it.
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    //Get the number of rows in the database.
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    return userCount == 1; //If a row is present in the UserInfo table, the user is returning.
                } catch (SQLiteException e)
                {
                    Core.PrintDebug(e.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// This function attempts to create the database on first launch. This ensures a clean slate.
        /// </summary>
        /// <returns>Returns the success of creating the database.</returns>
        public bool CreateDatabase()
        {
            bool success = false;
            // Catch any errors that may occur.
            try
            {
                // Check if a file with the same name exists in the filesystem.
                if (File.Exists(this.fileName))
                {
                    //Attempt to delete the file, since we need the name.
                    try
                    {
                        File.Delete(this.fileName);
                    } catch (IOException e)
                    {
                        Core.PrintDebug(e.Message);
                    }
                }
                //Create the new database file.
                SQLiteConnection.CreateFile(this.fileName);
                //using statement ensures the connection gets disposed of.
                using (var sqlite3 = new SQLiteConnection(this.connString))
                {
                    sqlite3.Open(); //Open the connection to the database.
                    string userSql = @"CREATE TABLE 'UserInfo'(
`id` INTEGER PRIMARY KEY AUTOINCREMENT,
`firstName` TEXT NOT NULL,
`lastName` TEXT NOT NULL,
`masterPassword` TEXT NOT NULL,
`masterPasswordHash` TEXT NOT NULL,
`emailAddress` TEXT NOT NULL,
`deviceId` TEXT)";
                    //Build the SQLite command to create the UserInfo table.
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
                    //Build the SQLite command to create the Services table.
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

        /// <summary>
        /// This method gets the list of services from the database and returns it as
        /// an ObservableCollection.
        /// </summary>
        /// <returns>Returns an ObservableCollection of type Service.</returns>
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
                        // Assign all of the values from each row in the Services table to its own Service class.
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
                        // Add the current Service class to the ObservableCollection.
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

        /// <summary>
        /// Adds a new Service to the database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns>Returns a value stating whether or not the updating of the database was successful.</returns>
        public bool AddService(Service service)
        {
            bool success = false;
            string sql = @"INSERT INTO Services (serviceName, email, loginName, description, password, hash, lastUpdated, website)
VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8);";
            // Using statement will dispose of the connection.
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open(); // Open the connection to the database.
                    // Build the SQLite command.
                    SQLiteCommand command = new SQLiteCommand(conn);
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    //Add parameters to prevent SQL injections.
                    command.Parameters.Add(new SQLiteParameter("@param1", service.ServiceName));
                    command.Parameters.Add(new SQLiteParameter("@param2", service.Email));
                    command.Parameters.Add(new SQLiteParameter("@param3", service.UserName));
                    command.Parameters.Add(new SQLiteParameter("@param4", service.Description));
                    command.Parameters.Add(new SQLiteParameter("@param5", service.HashedPassword));
                    command.Parameters.Add(new SQLiteParameter("@param6", service.PasswordHash));
                    command.Parameters.Add(new SQLiteParameter("@param7", Core.DateTimeToUnixTimestamp(service.LastUpdated)));
                    command.Parameters.Add(new SQLiteParameter("@param8", service.Website));
                    command.ExecuteNonQuery();
                    return true; //No exceptions, so the query executed successfully.
                } catch (Exception e)
                {
                    Core.PrintDebug(e.Message);
                    return false; //The query didn't execute successfully.
                }
            }
        }

        /// <summary>
        /// Update an existing Service in the database.
        /// </summary>
        /// <param name="service"></param>
        /// <returns>Returns a value representing the success of the update of the database.</returns>
        public bool UpdateService(Service service)
        {
            bool success = false;
            string sql = @"UPDATE Services
SET serviceName=@param1, email=@param2, loginName=@param3, description=@param4, password=@param5, hash=@param6, lastUpdated=@param7, website=@param8
WHERE id = @param9;";
            // Using ensures the connection will be disposed of.
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open(); //Open the SQLite connection.
                    //Build the SQLite command.
                    SQLiteCommand command = new SQLiteCommand(conn);
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    //Add parameters, to prevent SQL injections.
                    command.Parameters.Add(new SQLiteParameter("@param1", service.ServiceName));
                    command.Parameters.Add(new SQLiteParameter("@param2", service.Email));
                    command.Parameters.Add(new SQLiteParameter("@param3", service.UserName));
                    command.Parameters.Add(new SQLiteParameter("@param4", service.Description));
                    command.Parameters.Add(new SQLiteParameter("@param5", service.HashedPassword));
                    command.Parameters.Add(new SQLiteParameter("@param6", service.PasswordHash));
                    command.Parameters.Add(new SQLiteParameter("@param7", Core.DateTimeToUnixTimestamp(service.LastUpdated)));
                    command.Parameters.Add(new SQLiteParameter("@param8", service.Website));
                    command.Parameters.Add(new SQLiteParameter("@param9", service.Id));
                    command.ExecuteNonQuery();
                    return true; //The query executed successfully.
                } catch (Exception e)
                {
                    return false; //The query failed.
                }
            }
        }

        /// <summary>
        /// Adds a new User to the database.
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>Returns a value representing the success of the operation.</returns>
        public bool CreateNewUser(UserInfo userInfo)
        {
            bool success = false;
            try
            {
                // Using statement ensures that the SQLite connection is disposed of
                using (var sqlite3 = new SQLiteConnection(this.connString))
                {
                    sqlite3.Open(); //Open the connection.
                    //Build the SQLite command.
                    SQLiteCommand command = new SQLiteCommand(sqlite3);
                    command.CommandText = @"INSERT INTO UserInfo (firstName, lastName, masterPassword, masterPasswordHash, emailAddress)
VALUES (@param1, @param2, @param3, @param4, @param5);";
                    command.CommandType = CommandType.Text;
                    //Add parameters to prevent SQL injections.
                    command.Parameters.Add(new SQLiteParameter("@param1", userInfo.Forename));
                    command.Parameters.Add(new SQLiteParameter("@param2", userInfo.Surname));
                    command.Parameters.Add(new SQLiteParameter("@param3", userInfo.MasterPassword));
                    command.Parameters.Add(new SQLiteParameter("@param4", userInfo.PasswordHash));
                    command.Parameters.Add(new SQLiteParameter("@param5", userInfo.EmailAddress));
                    command.ExecuteNonQuery();
                }
                success = true; //Command executed successfully.
            }
            catch (Exception e)
            {
                Core.PrintDebug(e.Message);
            }
            return success;
        }

        /// <summary>
        /// This function gets the details of the user from the UserInfo table.
        /// </summary>
        /// <returns>A dictionary with key-value pairs holding data from the database.</returns>
        public Dictionary<string, object> GetUserInfo()
        {
            string sql = @"select * from UserInfo;";
            DataTable dataTable = this.Select(sql);
            try
            {
                DataRow dr = dataTable.Rows[0]; //Get only the top row, since there should only be one.
                Dictionary<string, object> holder = new Dictionary<string, object>();
                // Add the key-value pairs to the new dictionary.
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
            return new Dictionary<string, object>(); //Return a blank dictionary if the database is blank.
        }

        /// <summary>
        /// Represents a SELECT statement on the database.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>A DataTable with the data retrieved from the database</returns>
        public DataTable Select(string sql)
        {
            DataTable dt = new DataTable();
            // Using statement ensures the connection is disposed of.
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open(); //Open the connection.
                    //Build the command and data readers.
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader); //Load the data from the reader into the DataTable.
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

        /// <summary>
        /// Deletes a Service from the database.
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns>A value indicating the success of the deletion operation.</returns>
        public bool DeleteService(int rowId)
        {
            bool success = false;
            // Using statement ensures the connection is disposed of.
            using (var conn = new SQLiteConnection(this.connString))
            {
                try
                {
                    conn.Open(); //Open the connection.
                    //Build the SQLite command.
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