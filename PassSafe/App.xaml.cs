using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Windows.Threading;
using PassSafe.Views;

namespace PassSafe
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

        private delegate void updateDelegate();

        private bool isFirstTimeUser;
        Timer t;

        //Windows
        Views.SplashScreen splashScreen;
        MainWindow mainWindow;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mainWindow = new MainWindow();
            mainWindow.Show();

            //if (System.Diagnostics.Process.GetProcessesByName(
            //    System.IO.Path.GetFileNameWithoutExtension(
            //        System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            //{
            //    MessageBoxResult reallyQuit = MessageBox.Show("Another instance of PassSafe is already running.",
            //    "PassSafe",
            //    MessageBoxButton.OK);
            //    if (reallyQuit == MessageBoxResult.OK)
            //        Current.Shutdown();
            //}

            //t = new Timer();
            //t.Interval = 5000;
            //t.Elapsed += T_Elapsed;

            //splashScreen = new SplashScreen();
            //splashScreen.Show();
            //t.Start();

            //mainWindow = new MainWindow();
        }
    }
}
