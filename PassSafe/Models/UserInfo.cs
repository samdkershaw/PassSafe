using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.Models
{
    class UserInfo : INotifyPropertyChanged
    {
        private static readonly Lazy<UserInfo> lazy = new Lazy<UserInfo>(() => new UserInfo());

        public static UserInfo Instance { get { return lazy.Value; } }

        private Dictionary<string, object> userInfoHolder = new Dictionary<string, object>();

        private UserInfo()
        {
            //this.userInfoHolder = 
        }

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
