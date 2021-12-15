using System;
using System.IO;
using ThingsToRemember.Services;
using ThingsToRemember.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember
{
    public partial class App : Application
    {
        #region "Properties"

        private static Database _database;

        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                          , "ThingsToRemember.db3");
                    _database = new Database(path);
                }

                return _database;
            }
        }

        public static MockDataStore MockData => new MockDataStore();

        #endregion

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQwNTIyQDMxMzkyZTMzMmUzMGlJOVkvcndVR0JzS09neUlqQTNHYTY5Y3hPdEhYYXZaaXYwZjhuNEhPblU9");
            InitializeComponent();
            
            DependencyService.Register<Database>();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        #region "Device states"
        
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        
        #endregion

    }
}
