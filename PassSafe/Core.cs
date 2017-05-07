using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Security;
using System.Runtime.InteropServices;

namespace PassSafe
{
    public static class Core
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

        public static bool IsUrlAcceptable(string _uri)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(_uri, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
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
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return (long)Math.Ceiling(date.Subtract(epoch).TotalSeconds);
        }

        public static string SecureStringToString(SecureString secureString)
        {
            IntPtr unmanagedBytes = IntPtr.Zero;
            try
            {
                unmanagedBytes = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedBytes);
            } finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedBytes);
            }
        }

        public static bool IsEqualTo(this SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }
    }
}