using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;
using PassSafe.Data;
using PassSafe.Views;
using System.Windows.Input;

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
            //this.Update();
            this.AddServiceCommand = new DelegateCommand(this.AddService);
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

        public void Update()
        {
            Database db = new Database();
            this.Services = new ObservableCollection<Service>(db.GetServices());
        }
    }
}
