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

namespace PassSafe.ViewModels
{
    class ViewModelMainWindow : ViewModelBase
    {
        public ObservableCollection<Service> ServicesList { get; set; }
        public DelegateCommand AddServiceCommand { get; private set; }
        public DelegateCommand EditServiceCommand { get; private set; }
        public DelegateCommand DeleteServiceCommand { get; private set; }

        public ViewModelMainWindow()
        {
            this.Update();
            this.AddServiceCommand = new DelegateCommand(this.AddService);
            this.EditServiceCommand = new DelegateCommand(this.EditService, CanEditOrDelete);
            this.DeleteServiceCommand = new DelegateCommand(this.DeleteService, CanEditOrDelete);
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
                }
            }
        }

        private void AddService()
        {
            AddServiceWindow win = new AddServiceWindow();
            win.ShowDialog();
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
        }

        private void DeleteService()
        {
            if (SelectedService == null) {
                MessageBox.Show("ERROR: You need to select a service in order to do that.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private bool CanEditOrDelete()
        {
            return SelectedService != null;
        }

        public void Update()
        {
            this.ServicesList = new Database().GetServices();
        }
    }
}
