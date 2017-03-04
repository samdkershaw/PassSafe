using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe
{
    sealed class PasswordFactory
    {
        private string prevPassword;
        private int length;
        private const byte MinAsciiChar = 32;
        private const byte MaxAsciiChar = 255;

        public PasswordFactory(string mPrevPassword="", int mLength=-1)
        {
            prevPassword = mPrevPassword;
            length = (mLength > -1) ? mLength : GenerateRandomLength();
        }

        public string Generate()
        {
            return CreatePasswordFromList();
        }

        private string CreatePasswordFromList()
        {
            List<char> password = new List<char>();
            Random rand = new Random();
            for (int i = 1; i <= length; i++)
                password.Add(Convert.ToChar(rand.Next(MinAsciiChar, MaxAsciiChar)));

            return password.ToString();
        }

        private int GenerateRandomLength()
        {
            return new Random().Next(6, 20);
        }


    }
}
