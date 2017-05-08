using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using PassSafe.ViewModels;

namespace PassSafe.Views
{
    /// <summary>
    /// Interaction logic for ReEnterPasswordWindow.xaml
    /// </summary>
    public partial class ReEnterPasswordWindow : MetroWindow
    {
        public ReEnterPasswordWindow()
        {
            InitializeComponent();

            ViewModelReEnterPasswordWindow vm = new ViewModelReEnterPasswordWindow();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.CloseAndOpen);
        }

        private void CloseAndOpen()
        {
            Properties.Settings.Default.LastLogin = DateTime.Now;
            Properties.Settings.Default.Save();
            new MainWindow().Show();
            this.Close();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}