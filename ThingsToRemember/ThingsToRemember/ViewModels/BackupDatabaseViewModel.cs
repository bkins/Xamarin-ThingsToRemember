using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avails.D_Flat;
using Avails.Xamarin;
using ThingsToRemember.Services;

namespace ThingsToRemember.ViewModels
{
    public class BackupDatabaseViewModel : BaseViewModel
    {
        public  string                DatabaseLocation => DataAccessLayer.DatabaseLocation;
        public  string                DatabaseName     => DataAccessLayer.DatabaseFileName;
        public  string                BackupLocation   { get; set; }

        private BackupRestoreDatabase BackupRestorer   { get; set; }

        public BackupDatabaseViewModel(string destinationPath = "")
        {
            BackupLocation = destinationPath;
            
            if (destinationPath.IsNullEmptyOrWhitespace())
            {
                BackupLocation = Path.Combine(DatabaseLocation
                                            , "Backups");
            }

            BackupRestorer = new BackupRestoreDatabase(DatabaseLocation
                                                     , BackupLocation);
        }

        public string Backup()
        {
            //DataAccessLayer.BackupDatabase();
            
            var backedUpFileNameAndPath = BackupRestorer.Backup(DatabaseName);

            return backedUpFileNameAndPath;
        }

        public void Restore(string fileToRestoreFileName)
        {
            //BackupRestorer.Restore(fileToRestoreFileName);
            var destinationDatabase = App.BackupDatabase;
            
            destinationDatabase.RestoreDatabaseFromDestination();
        }

        public static string BackupDataFromSource()
        {
            var destinationDatabase = App.BackupDatabase;
            
            destinationDatabase.BackupDatabaseFromSource();

            return destinationDatabase.GetFilePath();
        }

        public static void Restore()
        {
            //BENDO: Give user option to choose the backed up database to restore vis:
            // App.DefaultBackupFileName = "<backed up database name>";
            
            var destinationDatabase = App.BackupDatabase;
            
            destinationDatabase.RestoreDatabaseFromDestination();
        }
    }
}
