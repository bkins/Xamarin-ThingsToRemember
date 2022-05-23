using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Avails.Xamarin;
using Avails.Xamarin.Interfaces;
using ThingsToRemember.Services;
using Xamarin.Forms;

namespace ThingsToRemember
{
    public partial class App : Application
    {
        #region "Properties"

        public static string DefaultBackupFileName { get; set; } = "ThingsToRemember.db3";
        
        private static Database _backupDatabase;

        public static Database BackupDatabase
        {
            get
            {
                if (_backupDatabase ==null)
                {
                    var path = Path.Combine(DependencyService.Get<IDependencyService>()
                                                             .GetExternalStorage()
                                          , DefaultBackupFileName);

                    var fileInfo    = new FileInfo(path);

                    var isReadyOnly = fileInfo.IsReadOnly;
                    var dirExists   = fileInfo.Directory is
                    {
                        Exists: true
                    };
                    
                    if ( ! File.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        File.Create(path);
                    }
                    _backupDatabase = new Database(path);
                }

                return _backupDatabase;
            }
        }
        private static Database _database;

        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                          , DefaultBackupFileName);
                    
                    if ( ! File.Exists(path))
                    {
                        File.Create(path);
                    }
                    
                    _database = new Database(path);
                }

                return _database;
            }
        }

        public static  MockDataStore MockData => new MockDataStore();

        #endregion

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQ5NjAxQDMxMzkyZTM0MmUzMGxzOS91RnJkRzRVUzIrYjNvWTZOcjcraWJYd2NyRnNvTUpCS3ZTWVlsYWc9");
            
            InitializeComponent();
            
            DependencyService.Register<Database>();
            DependencyService.Register<MockDataStore>();

            Logger.WriteToToast = false;
            
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
