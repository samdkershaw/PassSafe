using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PassSafe
{
    public class FileIO
    {
        public static bool FileExistsInAppDirectory(string fileName)
        {
            if (File.Exists(Core.GetAppDirectory() + fileName))
            {
                return true;
            }
            return false;
        }
    }
}
