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
        public ObservableCollection<Service> Services { get; set; }
        public DelegateCommand AddServiceCommand { get; private set; }
        public DelegateCommand EditServiceCommand { get; private set; }
        public DelegateCommand DeleteServiceCommand { get; private set; }

        public ViewModelMainWindow()
        {
            this.Update();
            this.AddServiceCommand = new DelegateCommand(this.AddService);
            this.EditServiceCommand = new DelegateCommand(this.EditService);
            this.DeleteServiceCommand = new DelegateCommand(this.DeleteService);
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
            Core.PrintDebug(SelectedService.Id.ToString());
            if (SelectedService == null || SelectedService.Id == 0)
            {
                MessageBox.Show("Error");
                return;
            }
        }

        private void DeleteService()
        {
            if (SelectedService == null || SelectedService.Id == 0) {
                MessageBox.Show("Error");
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
                    Update();
                }
            }
        }

        public void Update()
        {
            Database db = new Database();
            this.Services = new ObservableCollection<Service>(db.GetServices());
        }
    }
}
