using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassSafe.Data;
using PassSafe.Models;

namespace PassSafe.ViewModels
{
    class ViewModelAddServiceWindow : ViewModelBase
    {
        public DelegateCommand SubmitCommand { get; private set; }

        public ViewModelAddServiceWindow()
        {
            this.SubmitCommand = new DelegateCommand(this.Submit, this.CanChangesBeSubmitted);
            this.NewService = new Service();
        }

        Service _NewService;
        public Service NewService
        {
            get
            {
                return _NewService;
            }
            set
            {
                _NewService = value;
                OnPropertyChanged();
            }
        }

        private void Submit()
        {

        }

        private bool CanChangesBeSubmitted()
        {
            return false;
        }
    }
}
