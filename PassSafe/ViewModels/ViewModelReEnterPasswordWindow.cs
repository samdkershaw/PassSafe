using PassSafe.Encryption;
using PassSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PassSafe.ViewModels
{
    class ViewModelReEnterPasswordWindow : ViewModelBase
    {
        public DelegateCommand SubmitCommand { get; private set; }
        public Action CloseAction { get; set; }
        UserInfo userInfo;
        string TfaCode;

        public ViewModelReEnterPasswordWindow()
        {
            this.SubmitCommand = new DelegateCommand(this.Submit);
            this.ErrorsVisibility = Visibility.Collapsed;
            this.userInfo = new UserInfo();
            this.TfaCode = new TwoFactorAuthentication(userInfo.EmailAddress, userInfo.Forename).TwoFactorCode;
        }

        private void Submit()
        {
            if (Password != null)
            {
                MasterPasswordHasher hasher = new MasterPasswordHasher();
                hasher.HashedPassword = userInfo.MasterPassword;
                hasher.SaltValue = userInfo.PasswordHash;
                if (hasher.VerifyPassword(Core.SecureStringToString(Password))
                    && (TwoFactorCode == this.TfaCode))
                    this.CloseAction();
                else
                    ErrorsVisibility = Visibility.Visible;
            }
        }

        Visibility _ErrorsVisibility;
        public Visibility ErrorsVisibility
        {
            get { return _ErrorsVisibility; }
            set
            {
                SetProperty(ref _ErrorsVisibility, value);
            }
        }

        string _TwoFactorCode;
        public string TwoFactorCode
        {
            get { return _TwoFactorCode; }
            set
            {
                SetProperty(ref _TwoFactorCode, value);
            }
        }

        SecureString _Password;
        public SecureString Password
        {
            get { return _Password; }
            set
            {
                SetProperty(ref _Password, value);
            }
        }
    }
}
