using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.ViewModels
{
    class ViewModelReEnterPasswordWindow : ViewModelBase
    {
        public DelegateCommand SubmitCommand { get; private set; }

        public ViewModelReEnterPasswordWindow()
        {
            this.SubmitCommand = new DelegateCommand(this.Submit);
        }

        private void Submit()
        {
            //Add shite
        }

        SecureString _Password;
        public SecureString Password
        {
            get { return _Password; }
            set
            {
                SetProperty(ref _Password, value);
            }
        }
    }
}
