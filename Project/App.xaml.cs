using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // When the application is run, we need to decide which Window to open.
        //  This will depend on a number of factors, including:
        //      # First Run or not
        //      # Google Sign In is valid
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            new MainWindow().Show();
        }
    }
}
