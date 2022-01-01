using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avails.Xamarin;

namespace ThingsToRemember.ViewModels
{
    public class BackupDatabaseViewModel : BaseViewModel
    {
        public  string                DatabaseLocation => DataAccessLayer.DatabaseLocation;
        public  string                DatabaseName     => DataAccessLayer.DatabaseFileName;
        public  string                BackupLocation   { get; set; }

        private BackupRestoreDatabase BackupRestorer   { get; set; }

        public BackupDatabaseViewModel(string destinationPath)
        {
            BackupLocation = destinationPath;

            BackupRestorer = new BackupRestoreDatabase(DatabaseLocation
                                                     , BackupLocation);
        }

        public string Backup()
        {
             var backedUpFileNameAndPath = BackupRestorer.Backup(DatabaseName);

             return backedUpFileNameAndPath;
        }

        public void Restore(string fileToRestoreFileName)
        {
            BackupRestorer.Restore(fileToRestoreFileName);
        }
    }
}
