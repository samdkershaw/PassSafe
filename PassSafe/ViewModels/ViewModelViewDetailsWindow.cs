using PassSafe.Encryption;
using PassSafe.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.ViewModels
{
    class ViewModelViewDetailsWindow : ViewModelBase
    {
        public DelegateCommand ViewWebsite { get; private set; }

        public ViewModelViewDetailsWindow(Service selectedService)
        {
            this.SelectedService = selectedService;
            this.ViewWebsite = new DelegateCommand(this.OpenWebsite);
        }

        Service _SelectedService;
        public Service SelectedService
        {
            get { return _SelectedService; }
            set
            {
                SetProperty(ref _SelectedService, value);
            }
        }

        public string Password
        {
            get { return PasswordCipher.Decrypt(SelectedService.HashedPassword, new UserInfo().MasterPassword); }
        }

        private void OpenWebsite()
        {
            Process.Start(new ProcessStartInfo(SelectedService.Website));
        }
    }
}
