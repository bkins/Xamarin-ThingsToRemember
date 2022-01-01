﻿using System;
using System.IO;
using ThingsToRemember.Services;
using Xamarin.Forms;

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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQ5NjAxQDMxMzkyZTM0MmUzMGxzOS91RnJkRzRVUzIrYjNvWTZOcjcraWJYd2NyRnNvTUpCS3ZTWVlsYWc9");
            
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
