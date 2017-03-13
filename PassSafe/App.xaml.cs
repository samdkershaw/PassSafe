using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using System.Windows.Threading;

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
        SplashScreen splashScreen;
        MainWindow mainWindow;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            /*const int MIN_SPLASH_TIME = 4000;
            base.OnStartup(e);

            SplashScreen screen = new SplashScreen("SplashScreen.png");
            screen.Show(false);
            screen.Close(TimeSpan.FromSeconds(5));

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
            mainWindow.Show();*/

            base.OnStartup(e);

            t = new Timer();
            t.Interval = 5000;
            t.Elapsed += T_Elapsed;

            splashScreen = new SplashScreen();
            splashScreen.Show();
            t.Start();

            mainWindow = new MainWindow();
        }

        private bool IsFirstTimeUser()
        {
            return !(FileIO.FileExistsInAppDirectory("user.db"));
        }

        private void T_Elapsed(object sender, ElapsedEventArgs args)
        {
            Dispatcher.BeginInvoke(
          DispatcherPriority.Background,
          new Action(() => this.TimerElapsed()));
            t.Dispose();
        }

        private void TimerElapsed()
        {
            splashScreen.Close();
            if (isFirstTimeUser)
            {
                mainWindow.Show();
            } else
            {
                mainWindow.Show();
            }
        }
    }
}
