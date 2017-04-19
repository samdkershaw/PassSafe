using System;
using System.Windows;
using MahApps.Metro.Controls;
using PassSafe.ViewModels;

namespace PassSafe.Views
{
    /// <summary>
    /// Interaction logic for AddServiceWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : MetroWindow
    {
        public AddServiceWindow()
        {
            InitializeComponent();
            ViewModelAddServiceWindow vm = new ViewModelAddServiceWindow();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);

            this.txtServiceName.Focus();
        }

        private void AddServiceWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel? You'll lose your changes.",
                                                        "Cancel?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }
    }
}
