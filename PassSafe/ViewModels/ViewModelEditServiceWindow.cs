using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;
using PassSafe.Views;
using System.Collections.ObjectModel;
using System.Security;

namespace PassSafe.ViewModels
{
    class ViewModelEditServiceWindow : ViewModelBase
    {
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand GeneratePassword { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<string> ErrorsList { get; set; }

        public ViewModelEditServiceWindow(Service selectedService)
        {
            this.SubmitCommand = new DelegateCommand(this.Submit);
            this.GeneratePassword = new DelegateCommand(this.CreateSecurePassword);
            this.CancelCommand = new DelegateCommand(this.CloseWindow);
            this.ErrorsList = new ObservableCollection<string>();
            this.PasswordBoxEnabled = true;
            this.SelectedService = selectedService;
        }

        Service _SelectedService;
        public Service SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                _SelectedService = value;
                OnPropertyChanged();
            }
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

        //string _ServiceName;
        //public string ServiceName
        //{
        //    get { return _ServiceName; }
        //    set
        //    {
        //        SetProperty(ref _ServiceName, value);
        //    }
        //}

        //string _LoginName;
        //public string LoginName
        //{
        //    get { return _LoginName; }
        //    set
        //    {
        //        SetProperty(ref _LoginName, value);
        //    }
        //}

        //string _EmailAddress;
        //public string EmailAddress
        //{
        //    get { return _EmailAddress; }
        //    set
        //    {
        //        SetProperty(ref _EmailAddress, value);
        //    }
        //}

        //string _Website;
        //public string Website
        //{
        //    get { return _Website; }
        //    set
        //    {
        //        SetProperty(ref _Website, value);
        //    }
        //}

        //string _Description;
        //public string Description
        //{
        //    get { return _Description; }
        //    set
        //    {
        //        SetProperty(ref _Description, value);
        //    }
        //}

        string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                SetProperty(ref _Password, value);
            }
        }

        private void Submit()
        {
            
        }

        private bool CanChangesBeSaved()
        {
            return false;
        }

        public string TitleText
        {
            get
            {
                if (!(SelectedService == null))
                    return String.Format("Editing '{0}'", SelectedService.ServiceName);
                else
                    return "";
            }
        }

        public string WindowDescription
        {
            get
            {
                if (!(SelectedService == null))
                {
                    return String.Format("You are currently editing the service '{0}'."
                        + " Any changes you make will be visible once you click 'Save Changes'.\n\n"
                        + "You can press 'Cancel' to safely head back to the previous window.", SelectedService.ServiceName);
                } else
                {
                    return "";
                }
            }
        }

        private bool IsInputValid()
        {
            this.ErrorsList.Clear();
            bool serviceNameValid = false, loginNameValid = false, emailAddressValid = false,
                websiteValid = false, passwordValid = false;

            if (!String.IsNullOrEmpty(SelectedService.ServiceName))
                serviceNameValid = true;
            else
                this.ErrorsList.Add("The service name can't be empty.");

            if (!String.IsNullOrEmpty(SelectedService.UserName))
                loginNameValid = true;
            else
                this.ErrorsList.Add("The login name can't be empty.");

            if (!String.IsNullOrEmpty(SelectedService.Email) && Core.IsEmailAcceptable(SelectedService.Email))
                emailAddressValid = true;
            else
                this.ErrorsList.Add("The email address was invalid");

            if (!String.IsNullOrEmpty(SelectedService.Website) && Core.IsUrlAcceptable(SelectedService.Website))
                websiteValid = true;
            else
                this.ErrorsList.Add("The website was invalid.");

            // Description isn't required to have a value entered.
            if (String.IsNullOrEmpty(SelectedService.Description))
                SelectedService.Description = "";

            if (!String.IsNullOrEmpty(SelectedService.HashedPassword) && SelectedService.HashedPassword.Length >= 6)
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
    }
}
