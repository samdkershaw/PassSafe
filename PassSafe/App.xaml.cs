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

        //Windows
        Window childWindow;
        string[] colours = { "Red", "Blue", "Green", "Indigo" };


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Random rand = new Random();
            string colour = colours[rand.Next(0, colours.Length - 1)];

            ThemeManager.ChangeAppStyle(Current,
                                        ThemeManager.GetAccent(colour),
                                        ThemeManager.GetAppTheme("BaseLight"));

            Database db = new Database();
            int attempts = 0;
            do
            {
                if (db.DoesDatabaseExist())
                {
                    // Check whether the user exists in the database.
                    if (db.IsReturningUser())
                    {
                        // Let the user log in if they only logged in in the last 5 minutes
                        if (PassSafe.Properties.Settings.Default.LastLogin > DateTime.Now.AddMinutes(5))
                            childWindow = new MainWindow();
                        else //Otherwise, force password re-entry and Two-Factor Authentication.
                            childWindow = new ReEnterPasswordWindow();
                    } else
                    {
                        // The user doesn't have an account, so register them.
                        childWindow = new NewUserWindow();
                    }
                    childWindow.Show();
                    break;
                }
                else
                {
                    db.CreateDatabase();
                }
            } while (++attempts < 3);

            if (childWindow == null)
            {
                MessageBoxResult quit = MessageBox.Show("A problem occured with the database...\nCouldn't create or open it after 3 attempts.",
                    "Database Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Shutdown();
            }
        }

        public void OpenMainWindow()
        {
            if (childWindow.IsLoaded)
            {
                new MainWindow().Show();
                childWindow.Close();
            }
        }
    }
}