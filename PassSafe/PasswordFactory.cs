using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;
using System.Security;

namespace PassSafe
{
    public class PasswordFactory
    {
        #region Variables
        private int length;
        private const byte MinAsciiChar = 33;
        private const byte MaxAsciiChar = 126;
        #endregion

        #region Constructor
        // Constructor for the PasswordFactory class.
        // A password length can be specified, or a random one will be chosen.
        public PasswordFactory(int mLength=-1)
        {
            // If the length is specified, assign it to the variable. Otherwise,
            //  generate a random length.
            length = (mLength > -1) ? mLength : GenerateRandomLength();
        }
        #endregion

        #region Methods
        // This method is called and returns a new, randomly generated password.
        public string Generate()
        {
            return CreatePassword();
        }

        // This method actually generates the password. It uses the Random class
        //  to generate a random ASCII number which will correspond to a char value.
        //   These are all then appended together.
        private string CreatePassword()
        {
            string password = "";
            Random rand = new Random();
            for (int i = 1; i <= length; i++)
                password += (Convert.ToChar(rand.Next(MinAsciiChar, MaxAsciiChar)));

            return password;
        }

        // Get a random number between 10 and 20, for the length of the password.
        private int GenerateRandomLength()
        {
            return new Random().Next(10, 20);
        }
        #endregion
    }
}
