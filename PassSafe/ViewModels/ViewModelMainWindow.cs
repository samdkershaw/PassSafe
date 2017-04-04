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
                    SetProperty(ref _SelectedService, value);
                }
            }
        }

        public ViewModelMainWindow()
        {
            Data.Database db = new Data.Database();
            Services = new ObservableCollection<Service>(db.GetServices());
        }
    }
}
