using College.Y.Services;
using College.Y.ViewModels;
using College.Y.Views;
using Plugin.LocalNotification;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace College.Y
{
    public partial class App : Application
    {
        //initiation
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            NotificationsOn().Wait();
        }
        //connection
        static SQLiteHelper database;
        public static SQLiteHelper Database
        {
            get
            {
                if (database == null)
                {
                    database = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyCollege.db3"));
                }
                return database;
            }
        }
        //notification
        private async Task NotificationsOn()
        {
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

        }
    }
}
