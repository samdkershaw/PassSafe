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
            menuEdit_Purge.Click += MenuEdit_Purge_Click;
            menuHelp_About.Click += MenuHelp_About_Click;
            menuHelp_Settings.Click += MenuHelp_Settings_Click;
            Closing += MainWindow_Closing;

            
        }

        private void MenuEdit_Purge_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msg = MessageBox.Show(@"Are you sure you want to do this? It's irreversible.",
                "Purge Database", MessageBoxButton.YesNo);
            if (msg == MessageBoxResult.Yes)
            {
                //Core.Purge();
                Update();
            } else
            {
                return;
            }
        }

        private void Update(bool updateDatabase = true)
        {
            // Update the page, data stores etc.
            MessageBox.Show("Update");
        }

        private void MenuHelp_Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
            Update();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult reallyQuit = MessageBox.Show("Are you sure you want to quit?",
                "Really Quit?",
                MessageBoxButton.YesNo);
            if (reallyQuit == MessageBoxResult.Yes)
                Application.Current.Shutdown();
            else
                e.Cancel = true;
        }

        private void MenuHelp_About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void MenuFile_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    class ServicesListItem
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
