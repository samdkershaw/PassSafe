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
            const int MIN_SPLASH_TIME = 4000;
            base.OnStartup(e);

            /*SplashScreen screen = new SplashScreen("SplashScreen.png");
            screen.Show(false);
            screen.Close(TimeSpan.FromSeconds(5));*/

            SplashScreen screen = new SplashScreen();
            screen.Show();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            MainWindow mainWindow = new MainWindow();
            timer.Stop();
            int timeRemaining = MIN_SPLASH_TIME - (int)timer.ElapsedMilliseconds;
            if (timeRemaining > 0)
            {
                System.Threading.Thread.Sleep(timeRemaining);
            }

            screen.Close();
            mainWindow.Show();
        }
    }
}
