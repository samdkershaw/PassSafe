using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

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
                var addr = new System.Net.Mail.MailAddress(_email);
                return addr.Address == _email;
            } catch
            {
                return false;
            }
        }
    }
}
