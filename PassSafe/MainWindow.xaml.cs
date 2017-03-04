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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace PassSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set up OnClick listeners for the menu bar.
            menuFile_Exit.Click += MenuFile_Exit_Click;
            menuHelp_About.Click += MenuHelp_About_Click;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ConfirmExit();
        }

        private void MenuHelp_About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void MenuFile_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmExit();
        }

        private void ConfirmExit()
        {
            MessageBoxResult reallyQuit = MessageBox.Show("Are you sure you want to quit?",
                "Really Quit?",
                MessageBoxButton.YesNo);
            if (reallyQuit == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}
