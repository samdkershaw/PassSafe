using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;
using PassSafe.Data;
using PassSafe.Views;
using System.Windows.Input;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace PassSafe.ViewModels
{
    class ViewModelMainWindow : ViewModelBase
    {
        public DelegateCommand AddServiceCommand { get; private set; }
        public DelegateCommand EditServiceCommand { get; private set; }
        public DelegateCommand DeleteServiceCommand { get; private set; }
        public DelegateCommand ViewDetailsCommand { get; private set; }
        public DelegateCommand OpenURL { get; private set; }

        public ViewModelMainWindow()
        {
            this.Update();
            this.AddServiceCommand = new DelegateCommand(this.AddService);
            this.EditServiceCommand = new DelegateCommand(this.EditService, CanEditOrDelete);
            this.DeleteServiceCommand = new DelegateCommand(this.DeleteService, CanEditOrDelete);
            this.ViewDetailsCommand = new DelegateCommand(this.ViewServiceDetails, CanEditOrDelete);
            this.OpenURL = new DelegateCommand(this.OpenBrowser);
            this.UserInfoHolder = new UserInfo();
        }

        private void OpenBrowser(object _url)
        {
            string url = (string)_url;
            Core.PrintDebug(url);
        }

        ObservableCollection<Service> _ServicesList;
        public ObservableCollection<Service> ServicesList
        {
            get { return _ServicesList; }
            set
            {
                SetProperty(ref _ServicesList, value);
            }
        }

        UserInfo _UserInfoHolder;
        public UserInfo UserInfoHolder
        {
            get
            {
                return _UserInfoHolder;
            }
            set
            {
                SetProperty(ref _UserInfoHolder, value);
            }
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
                if (_SelectedService != value)
                {
                    SetProperty(ref _SelectedService, value);
                    EditServiceCommand.RaiseCanExecuteChanged();
                    DeleteServiceCommand.RaiseCanExecuteChanged();
                    ViewDetailsCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void AddService()
        {
            AddServiceWindow win = new AddServiceWindow();
            win.ShowDialog();
            this.Update();
        }

        private void EditService()
        {
            if (SelectedService == null)
            {
                MessageBox.Show("Error");
                return;
            }

            Core.PrintDebug(SelectedService.Id.ToString());

            new EditServiceWindow(SelectedService).ShowDialog();

            this.Update();
        }

        private void DeleteService()
        {
            if (SelectedService == null) {
                MessageBox.Show("ERROR: You need to select a service in order to do that.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show(String.Format("Are you sure you want to delete {0}? It can't be reversed.", SelectedService.ServiceName),
                                                        String.Format("Delete {0}?", SelectedService.ServiceName),
                                                        MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Database db = new Database();
                if (db.DeleteService(SelectedService.Id))
                {
                    ServicesList.Remove(SelectedService);
                }
            }
        }

        private void ViewServiceDetails()
        {
            if (SelectedService == null)
            {
                return;
            }

            Core.PrintDebug("View Details");
        }

        private bool CanEditOrDelete()
        {
            return SelectedService != null;
        }

        public void Update()
        {
            Database db = new Database();
            if (this.ServicesList != null)
                this.ServicesList = null;
            this.ServicesList = db.GetServices();
        }
    }
}
