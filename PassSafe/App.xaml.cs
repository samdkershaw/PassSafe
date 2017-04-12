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
using MahApps.Metro;
using PassSafe.Data;

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

            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("Blue"),
                                        ThemeManager.GetAppTheme("BaseLight"));

            if (new Database().DoesDatabaseExist())
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            } else
            {

            }
        }
    }
}
