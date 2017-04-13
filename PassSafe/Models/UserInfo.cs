using PassSafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.Models
{
    public class UserInfo : INotifyPropertyChanged
    {
        int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                RaisePropertyChanged("Id");
            }
        }

        string _Forename;
        public string Forename
        {
            get { return _Forename; }
            set
            {
                _Forename = value;
                RaisePropertyChanged("Forename");
            }
        }

        string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                _Surname = value;
                RaisePropertyChanged("Surname");
            }
        }

        string _MasterPassword;
        public string MasterPassword
        {
            get { return _MasterPassword; }
            set
            {
                _MasterPassword = value;
                RaisePropertyChanged("MasterPassword");
            }
        }

        string _PasswordHash;
        public string PasswordHash
        {
            get { return _PasswordHash; }
            set
            {
                _PasswordHash = value;
                RaisePropertyChanged("PasswordHash");
            }
        }

        string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set
            {
                _EmailAddress = value;
                RaisePropertyChanged("EmailAddress");
            }
        }

        public UserInfo(bool readFromDb=true)
        {
            if (readFromDb)
            {
                Database db = new Database();
                foreach (KeyValuePair<string, object> kvp in db.GetUserInfo())
                {
                    switch (kvp.Key)
                    {
                        case "Id":
                            Id = (int)kvp.Value;
                            break;
                        case "Forename":
                            Forename = kvp.Value.ToString();
                            break;
                        case "Surname":
                            Surname = kvp.Value.ToString();
                            break;
                        case "MasterPassword":
                            MasterPassword = kvp.Value.ToString();
                            break;
                        case "PasswordHash":
                            PasswordHash = kvp.Value.ToString();
                            break;
                        case "EmailAddress":
                            EmailAddress = kvp.Value.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
