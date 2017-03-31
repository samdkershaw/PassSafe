using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Models;

namespace PassSafe.ViewModels
{
    class ViewModelMainWindow : ViewModelBase
    {
        public ObservableCollection<Service> Services { get; set; }

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
                    _SelectedService = value;
                    RaisePropertyChanged("SelectedService");
                }
            }
        }

        public ViewModelMainWindow()
        {
            Services = new ObservableCollection<Service>
            {
                new Models.Service { ServiceName="facebook" },
            };
        }
    }
}
