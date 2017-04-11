using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;

namespace PassSafe
{
    public class Core
    {
        public static void PrintErrorToFile(string msg)
        {
            string logLocation = GetAppDirectory() + @"Log.txt";
            try
            {
                using (StreamWriter file = new StreamWriter(logLocation))
                {
                    file.WriteLine("[" + DateTime.Now.ToShortDateString() + "] Error: " + msg);
                    file.WriteLine(logLocation);
                }
            } catch (Exception)
            {
                
            }
        }

        public static string GetAppDirectory()
        {
            try
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\";
            }
            catch (Exception e)
            {
                Core.PrintErrorToFile(e.Message);
                return null;
            }
        }

        public static bool IsEmailAcceptable(string _email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(_email).Address == _email;
            } catch
            {
                return false;
            }
        }

        public static void PrintDebug(string msg)
        {
            Debug.WriteLine(msg);
        }

        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            return dtDateTime;
        }

        public static long DateTimeToUnixTimestamp(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return date.Subtract(epoch).Seconds;
        }
    }
}
