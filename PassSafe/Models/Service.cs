﻿using System;
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
        int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (value >= 0)
                    _Id = value;
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
                // Convert the string to title case
                System.Globalization.TextInfo t = new System.Globalization.CultureInfo("en-us", false).TextInfo;
                _ServiceName = t.ToTitleCase(value);
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
                _Website = Uri.EscapeDataString(value);
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

        string _HashedPassword;
        public string HashedPassword
        {
            set
            {
                _HashedPassword = value;
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
                if (value.GetType() == typeof(string))
                    _PasswordHash = value;
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

            }
        }

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
