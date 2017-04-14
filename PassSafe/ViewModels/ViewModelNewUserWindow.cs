using PassSafe.Data;
using PassSafe.Models;
using PassSafe.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace PassSafe.ViewModels
{
    class ViewModelNewUserWindow : ViewModelBase
    {
        public DelegateCommand HelpCommand { get; private set; }
        public DelegateCommand SubmitCommand { get; private set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<string> ErrorsList { get; set; }

        public ViewModelNewUserWindow()
        {
            this.HelpCommand = new DelegateCommand(this.OpenHelpSite);
            this.SubmitCommand = new DelegateCommand(this.Submit);
            this.ErrorsList = new ObservableCollection<string>();
        }

        private void OpenHelpSite()
        {
            Process.Start(new ProcessStartInfo(@"http://www.samdkershaw.com/PassSafe/help.htm"));
        }

        private void Submit()
        {
            this.SubmitCommand.RaiseCanExecuteChanged();
            if (IsInputValid())
            {
                Database db = new Database();
                UserInfo userInfo = new UserInfo(false);
                userInfo.Forename = this.Forename;
                userInfo.Surname = this.Surname;
                userInfo.EmailAddress = this.EmailAddress;
                userInfo.MasterPassword = "";
                userInfo.PasswordHash = "";
                if (db.CreateNewUser(userInfo))
                {
                    Core.PrintDebug("Success!");
                    this.CloseAction();
                }
            }
            else
            {
                Core.PrintDebug("Error!");
            }
        }

        private bool IsInputValid()
        {
            this.ErrorsList.Clear();
            bool ForenameValid=false, SurnameValid=false, EmailAddressValid=false, PasswordValid=false, PasswordsMatch=false;

            if (!String.IsNullOrEmpty(Forename) && Forename.All(Char.IsLetter))
                ForenameValid = true;
            else
                this.ErrorsList.Add("The forename you entered was illegal.");

            if (!String.IsNullOrEmpty(Surname) && Surname.All(Char.IsLetter))
                SurnameValid = true;
            else
                this.ErrorsList.Add("The surname you entered was illegal.");

            if (!String.IsNullOrEmpty(EmailAddress) && (EmailAddress.Length > 5 && EmailAddress.Length < 70))
                EmailAddressValid = true;
            else
                this.ErrorsList.Add("The email address you entered was invalid.");

            if (Password != null && ReEnterPassword != null)
            {
                if (Password.Length >= 6)
                    PasswordValid = true;
                else
                    this.ErrorsList.Add("The password you entered was too short. Make sure it's longer than 6 characters.");

                if (Core.IsEqualTo(Password, ReEnterPassword))
                    PasswordsMatch = true;
                else
                    this.ErrorsList.Add("The passwords you entered don't match.");
            }

            bool valid = ForenameValid && SurnameValid && EmailAddressValid && PasswordValid && PasswordsMatch;
            this.Errors = !valid;
            return valid;
        }

        bool _Errors;
        public bool Errors
        {
            get { return _Errors; }
            set
            {
                SetProperty(ref _Errors, value);
            }
        }

        string _Forename;
        public string Forename
        {
            get { return _Forename; }
            set
            {
                SetProperty(ref _Forename, value);
                Core.PrintDebug(_Forename);
            }
        }

        string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                SetProperty(ref _Surname, value);
                Core.PrintDebug(_Surname);
            }
        }

        string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set
            {
                SetProperty(ref _EmailAddress, value);
                Core.PrintDebug(_EmailAddress);
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

        SecureString _ReEnterPassword;
        public SecureString ReEnterPassword
        {
            get { return _ReEnterPassword; }
            set
            {
                SetProperty(ref _ReEnterPassword, value);
            }
        }
    }
}