using System.IO;
using System.Threading.Tasks;
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

        public string BackupDataFromSource()
        {
            var databaseBackerUpper = new BackupDatabase(App.BackupDatabase
                                                       , App.Database);
            databaseBackerUpper.Backup();
            databaseBackerUpper.ValidateBackup();

            return databaseBackerUpper.GetDestinationDatabasePath();
        }

        public void Restore()
        {
            var databaseBackerUpper = new BackupDatabase(App.Database
                                                       , App.BackupDatabase);
            databaseBackerUpper.Backup();
            databaseBackerUpper.ValidateBackup();
        }
    }
}
