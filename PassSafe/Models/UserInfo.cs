using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.Models
{
    public sealed class UserInfo
    {
        private static readonly Lazy<UserInfo> lazy = new Lazy<UserInfo>(() => new UserInfo());

        public static UserInfo Instance { get { return lazy.Value; } }

        private UserInfo()
        {
        }
    }
}
