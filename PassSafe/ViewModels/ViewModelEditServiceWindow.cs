using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;
using PassSafe.Views;
using System.Collections.ObjectModel;
using System.Security;
using PassSafe.Data;
using PassSafe.Encryption;

namespace PassSafe.ViewModels
{
    class ViewModelEditServiceWindow : ViewModelBase
    {
        //Delegates, Actions and Collections
        public DelegateCommand SubmitCommand { get; private set; }
        public DelegateCommand GeneratePassword { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<string> ErrorsList { get; set; }

        //Constructor that sets up the ViewModel.
        public ViewModelEditServiceWindow(Service selectedService)
        {
            this.SubmitCommand = new DelegateCommand(this.Submit);
            this.GeneratePassword = new DelegateCommand(this.CreateSecurePassword);
            this.CancelCommand = new DelegateCommand(this.CloseWindow);
            this.ErrorsList = new ObservableCollection<string>();
            this.PasswordBoxEnabled = true;
            this.SelectedService = selectedService;
        }

        //Stores the Service currently being edited.
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

        //Calls the CloseAction Action.
        private void CloseWindow()
        {
            this.CloseAction();
        }

        //Generates a new, strong password using the PasswordFactory.
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

        string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                SetProperty(ref _Password, value);
            }
        }

        //When the Submit button is clicked, this method is called.
        private void Submit()
        {
            // Check that all of the fields have valid input.
            if (!IsInputValid())
                return;
            Database db = new Database();
            
            //Check if the password has changed or not.
            if (!String.IsNullOrEmpty(Password) && Password.Length > 0)
                SelectedService.HashedPassword = PasswordCipher.Encrypt(Password, new UserInfo().MasterPassword);
            SelectedService.LastUpdated = DateTime.Now.AddHours(-1); // The time on PCs is one hour ahead for some reason.
            //Attempt to update the service in the database, then close the window.
            if (db.UpdateService(SelectedService))
            {
                Core.PrintDebug(String.Format("Service {0} updated successfully.", SelectedService.ServiceName));
                this.CloseAction();
            }
            else
            {
                //Show the errors to the user.
                this.ErrorsList.Clear();
                this.ErrorsList.Add("An error occured.");
                this.Errors = true;
            }
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

        //Detects if each field in the window has valid input, and returns false if not.
        private bool IsInputValid()
        {
            this.ErrorsList.Clear();
            bool serviceNameValid = false, loginNameValid = false, emailAddressValid = false,
                websiteValid = false;

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

            if (!String.IsNullOrEmpty(SelectedService.Website) && (Core.IsUrlAcceptable(SelectedService.Website) || SelectedService.Website.StartsWith("www.")))
                websiteValid = true;
            else
                this.ErrorsList.Add("The website was invalid.");

            // Description isn't required to have a value entered.
            if (String.IsNullOrEmpty(SelectedService.Description))
                SelectedService.Description = "";

            bool valid = serviceNameValid && loginNameValid && emailAddressValid
                && websiteValid;
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
