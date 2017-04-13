using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassSafe.ViewModels
{
    class ViewModelNewUserWindow : ViewModelBase
    {
        public DelegateCommand HelpCommand { get; private set; }

        public ViewModelNewUserWindow()
        {
            this.HelpCommand = new DelegateCommand(this.OpenHelpSite);
        }

        private void OpenHelpSite()
        {
            Process.Start(new ProcessStartInfo(@"http://www.samdkershaw.com/PassSafe/help.htm"));
        }
    }
}
