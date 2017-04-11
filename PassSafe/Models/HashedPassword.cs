using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.Models
{
    public class HashedPassword
    {
        private string password;
        private string hash;

        public HashedPassword(string _password)
        {
            this.password = _password;
        }
    }
}
