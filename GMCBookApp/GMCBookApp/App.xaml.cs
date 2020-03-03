using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GMCBookApp.Data;



namespace GMCBookApp
{
    public partial class App : Application
    {
        static BookDatabase database;

        public static BookDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new BookDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Books.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

    protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
