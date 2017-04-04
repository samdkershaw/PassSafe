using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Security;

namespace PassSafe.Models
{
    class Service : INotifyPropertyChanged
    {
        string _ServiceName;
        public string ServiceName
        {
            get
            {
                return _ServiceName;
            }
            set
            {
                // Convert the string to title case
                System.Globalization.TextInfo t = new System.Globalization.CultureInfo("en-us", false).TextInfo;
                _ServiceName = t.ToTitleCase(value);
            }
        }

        string _Email;
        public string Email
        {
            get
            {
                return _Email;
            } set
            {
                if (Core.IsEmailAcceptable(value))
                    _Email = value;
            }
        }

        string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            } set
            {

            }
        }

        SecureString _Password;
        public SecureString Password
        {
            set
            {
                _Password = value;
            }
        }

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
