using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Security;

namespace PassSafe.Models
{
    public class Service : INotifyPropertyChanged
    {
        int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                RaisePropertyChanged("Id");
            }
        }
        
        string _ServiceName;
        public string ServiceName
        {
            get
            {
                return _ServiceName;
            }
            set
            {
                _ServiceName = value;
                RaisePropertyChanged("ServiceName");
            }
        }

        string _Website;
        public string Website
        {
            get
            {
                return _Website;
            }
            set
            {
                _Website = value;
                RaisePropertyChanged("Website");
                //_Website = Uri.EscapeDataString(value);
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
                {
                    _Email = value;
                    RaisePropertyChanged("Email");
                }
                else return;
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
                _UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                RaisePropertyChanged("Description");
            }
        }

        string _HashedPassword;
        public string HashedPassword
        {
            get
            {
                return _HashedPassword;
            }
            set
            {
                _HashedPassword = value;
                RaisePropertyChanged("HashedPassword");
            }
        }

        string _PasswordHash;
        public string PasswordHash
        {
            get
            {
                return _PasswordHash;
            }
            set
            {
                _PasswordHash = value;
                RaisePropertyChanged("PasswordHash");
            }
        }

        DateTime _LastUpdated;
        public DateTime LastUpdated
        {
            get
            {
                return _LastUpdated;
            }
            set
            {
                _LastUpdated = value;
                RaisePropertyChanged("LastUpdated");
            }
        }

        //public Service()
        //{
        //    Id = -1;
        //    ServiceName = "";
        //    Website = "";
        //    Email = "";
        //    UserName = "";
        //    Description = "";
        //    HashedPassword = "";
        //    PasswordHash = "";
        //    LastUpdated = default(DateTime);
        //}

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
