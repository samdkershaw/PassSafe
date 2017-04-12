using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;
using PassSafe.Views;

namespace PassSafe.ViewModels
{
    class ViewModelEditServiceWindow : ViewModelBase
    {
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand SubmitCommand { get; private set; }

        public ViewModelEditServiceWindow(Service _SelectedService)
        {
            SelectedService = _SelectedService;
            this.CancelCommand = new DelegateCommand(this.Cancel);
            this.SubmitCommand = new DelegateCommand(this.Submit, this.CanChangesBeSaved);
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

        private void Cancel()
        {
            Core.PrintErrorToFile(App.Current.Windows.Count.ToString());
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

        public string Description
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
    }
}
