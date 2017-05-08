using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Data;
using PassSafe.Models;
using System.Security;
using System.Collections.ObjectModel;
using System.Windows;
using PassSafe.Views;
using PassSafe.Encryption;

namespace PassSafe.ViewModels
{
    class ViewModelAddServiceWindow : ViewModelBase
    {
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand GeneratePassword { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<string> ErrorsList { get; set; }

        public ViewModelAddServiceWindow()
        {
            this.SubmitCommand = new DelegateCommand(this.Submit);
            this.GeneratePassword = new DelegateCommand(this.CreateSecurePassword);
            this.CancelCommand = new DelegateCommand(this.CloseWindow);
            this.ErrorsList = new ObservableCollection<string>();
            this.PasswordBoxEnabled = true;
        }

        private void CloseWindow()
        {
            this.CloseAction();
        }

        private void CreateSecurePassword()
        {
            this.Password = new PasswordFactory().Generate();
            this.PasswordBoxEnabled = false;
        }

        bool _PasswordBoxEnabled;
        public bool PasswordBoxEnabled
        {
            get { return _PasswordBoxEnabled; }
            set
            {
                SetProperty(ref _PasswordBoxEnabled, value);
            }
        }

        string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                SetProperty(ref _ServiceName, value);
            }
        }

        string _LoginName;
        public string LoginName
        {
            get { return _LoginName; }
            set
            {
                SetProperty(ref _LoginName, value);
            }
        }

        string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set
            {
                SetProperty(ref _EmailAddress, value);
            }
        }

        string _Website;
        public string Website
        {
            get { return _Website; }
            set
            {
                SetProperty(ref _Website, value);
            }
        }

        string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                SetProperty(ref _Description, value);
            }
        }

        string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                SetProperty(ref _Password, value);
                Core.PrintDebug(value);
            }
        }

        private void Submit()
        {
            if (!IsInputValid())
                return;
            Database db = new Database();
            Service service = new Service();
            //service.Id = -1;
            service.ServiceName = this.ServiceName;
            service.UserName = this.LoginName;
            service.Email = this.EmailAddress;
            service.Website = this.Website;
            service.HashedPassword = PasswordCipher.Encrypt(this.Password, new UserInfo().MasterPassword);
            service.PasswordHash = "";
            service.Description = this.Description;
            service.LastUpdated = DateTime.Now;
            if (db.AddService(service))
            {
                Core.PrintDebug(String.Format("Service {0} added successfully.", service.ServiceName));
                this.CloseAction();
            } else
            {
                this.ErrorsList.Clear();
                this.ErrorsList.Add("An error occured.");
                this.Errors = true;
            }
        }

        private bool IsInputValid()
        {
            this.ErrorsList.Clear();
            bool serviceNameValid = false, loginNameValid = false, emailAddressValid = false,
                websiteValid = false, passwordValid = false;

            if (!String.IsNullOrEmpty(ServiceName))
                serviceNameValid = true;
            else
                this.ErrorsList.Add("The service name can't be empty.");

            if (!String.IsNullOrEmpty(LoginName))
                loginNameValid = true;
            else
                this.ErrorsList.Add("The login name can't be empty.");

            if (!String.IsNullOrEmpty(EmailAddress) && Core.IsEmailAcceptable(EmailAddress))
                emailAddressValid = true;
            else
                this.ErrorsList.Add("The email address was invalid");

            if (!String.IsNullOrEmpty(Website) && Core.IsUrlAcceptable(Website))
                websiteValid = true;
            else
                this.ErrorsList.Add("The website was invalid.");

            // Description isn't required to have a value entered.
            if (String.IsNullOrEmpty(Description))
                Description = "";

            if (!String.IsNullOrEmpty(Password) && Password.Length >= 6)
                passwordValid = true;
            else
                this.ErrorsList.Add("The password you entered was too short.");

            bool valid = serviceNameValid && loginNameValid && emailAddressValid
                && websiteValid && passwordValid;
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

        private bool CanChangesBeSubmitted()
        {
            return false;
        }
    }
}
