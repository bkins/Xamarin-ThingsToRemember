using System;
using System.IO;
using System.Threading.Tasks;
using ApplicationExceptions;
using Avails.Xamarin;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;
using StringBuilder = System.Text.StringBuilder;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationView 
    {
        public int  SwipedMoodItem        { get; set; }
        public int  SwipedJournalTypeItem { get; set; }

        public  Mood        MoodToEdit        { get; set; }
        public  JournalType JournalTypeToEdit { get; set; }

        private ConfigurationViewModel  ViewModel      { get; set; }
        private BackupDatabaseViewModel BackupViewModel { get; set; }

        public string BackupFolderPath => GetBackupDestinationPath();

        private static string GetBackupDestinationPath()
        {
            var backupFolder = Path.Combine("storage"
                                          , "emulated"
                                          , "0"
                                          , "Downloads"
                                          , "BackupDb");

            if ( ! Directory.Exists(backupFolder))
            {
                //Directory.CreateDirectory(backupFolder);
            }

            return backupFolder;
        }

        public ConfigurationView()
        {
            InitializeComponent();

            ViewModel       = new ConfigurationViewModel();
            BackupViewModel = new BackupDatabaseViewModel(string.Empty); //(BackupFolderPath);
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Title = "Configuration";

                ViewModel             = new ConfigurationViewModel();
                
                MoodsListView.ItemsSource        = ViewModel.MoodViewModel.ObservableListOfMoods;
                JournalTypesListView.ItemsSource = ViewModel.JournalTypeViewModel.ObservableJournalTypes;
                
                MoodsListView.IsVisible = true;
                EditMoodGrid.IsVisible  = false;

            }
            catch (DuplicateRecordException duplicateRecordException)
            {
                DisplayAlert("Duplicate Record Error"
                           , duplicateRecordException.Message
                           , "OK");
            }
            catch (Exception exception)
            {
                DisplayAlert("Error"
                           , exception.Message
                           , "OK");
            }
        }

        private async void ClearDataButton_OnClicked(object    sender
                                                   , EventArgs e)
        {
            var userWouldLikeToClear = await AskUserToClearUserData();

            if (userWouldLikeToClear)
            {
                ViewModel.ClearUserData();
            }
            else
            {
                //Phew! That was close!
            }
        }

        private async Task<bool> AskUserToClearUserData()
        {
            var userWouldLikeToClear = await DisplayAlert("Clear All Journal Data!?"
                                                        , GetClearAllJournalDataMessage()
                                                        , "Yes"
                                                        , "No");
            return userWouldLikeToClear;
        }

        private static string GetClearAllJournalDataMessage()
        {
            var question = new StringBuilder();
            question.AppendLine("You are about to clear all journals and their data.");
            question.AppendLine("This cannot be undone.");
            question.AppendLine("All data will be lost!");
            question.AppendLine("Would you like to continue?");

            return question.ToString();
        }

        private async Task<bool> AskUserToClearAppData()
        {
            var question = new StringBuilder();
            question.AppendLine("You are about to clear all application (Moods & Journal Types) data.");
            question.AppendLine("This cannot be undone.");
            question.AppendLine("All data will be lost!");
            question.AppendLine("Would you like to continue?");

            var userWouldLikeToClear = await DisplayAlert("Clear All Application Data!?"
                                                        , question.ToString()
                                                        , "Yes"
                                                        , "No");
            return userWouldLikeToClear;
        }

        private async void ClearApplicationDataButton_OnClicked(object    sender
                                                              , EventArgs e)
        {
            var userWouldLikeToClear = await AskUserToClearAppData();

            if (userWouldLikeToClear)
            {
                ViewModel.ClearAppData();

                MoodsListView.ItemsSource        = ViewModel.MoodViewModel.ObservableListOfMoods;
                JournalTypesListView.ItemsSource = ViewModel.JournalTypeViewModel.ObservableJournalTypes;
            }
            else
            {
                //Phew! That was close!
            }
        }

        private void ListView_SwipeMoodEnded(object              sender
                                       , SwipeEndedEventArgs e)
        {
            SwipedMoodItem = e.ItemIndex;
        }

        private void LeftImage_DeleteMood_BindingContextChanged(object    sender
                                                              , EventArgs e)
        {
            if (sender is Image deleteImage)
            {
                (deleteImage.Parent as View)?.GestureRecognizers
                                             .Add(new TapGestureRecognizer
                                                  {
                                                      Command = new Command(DeleteMood)
                                                  });
            }
        }
        
        private async void DeleteMood()
        {
            var itemDeleted = ViewModel.MoodViewModel.DeleteMood(SwipedMoodItem);

            await DisplayAlert("Mood Deleted"
                             , itemDeleted
                             , "OK");

            MoodsListView.ItemsSource = ViewModel.MoodViewModel.ObservableListOfMoods;
            
            MoodsListView.ResetSwipe();
        }
        
        private async void DeleteJournalType()
        {
            var itemDeleted = ViewModel.JournalTypeViewModel.Delete(SwipedJournalTypeItem);

            await DisplayAlert("Journal Type Deleted"
                             , itemDeleted
                             , "OK");

            MoodsListView.ItemsSource = ViewModel.JournalTypeViewModel.ObservableJournalTypes;
            
            JournalTypesListView.ResetSwipe();
        }

        private void LeftImage_EditMood_BindingContextChanged(object    sender
                                                            , EventArgs e)
        {
            if (sender is Image editImage)
            {
                (editImage.Parent as View)?.GestureRecognizers
                                           .Add(new TapGestureRecognizer
                                                {
                                                    Command = new Command(EditMood)
                                                });
            }
        }
        
        private async void EditMood(object obj)
        {
            MoodToEdit = ViewModel.MoodViewModel.GetMoodToEdit(SwipedMoodItem);

            await PageNavigation.NavigateTo(nameof(EditMoodPopUp)
                                          , nameof(EditMoodPopUp.MoodId)
                                          , MoodToEdit.Id.ToString());
            
            if (PageCommunication.Instance.IntegerValue != 0)
            {
                ViewModel.MoodViewModel.RefreshListOfMoods();
                MoodsListView.ItemsSource = ViewModel.MoodViewModel.ObservableListOfMoods;
                
                PageCommunication.Instance.Clear();
            }
        }
        
        private void ToggleEditView()
        {
            MoodsListView.IsVisible = ! MoodsListView.IsVisible;
            EditMoodGrid.IsVisible  = ! EditMoodGrid.IsVisible;
        }

        private void DoneEditingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            MoodToEdit.Title = MoodTitleEntry.Text;
            MoodToEdit.Emoji = MoodEmojiEntry.Text;
            
            SaveMoodToEdit();

            ToggleEditView();
            
            ViewModel.MoodViewModel.RefreshListOfMoods();
            MoodsListView.ItemsSource = ViewModel.MoodViewModel.ObservableListOfMoods;
        }

        private void SaveMoodToEdit()
        {
            try
            {
                ViewModel.MoodViewModel.Save(MoodToEdit);
            }
            catch (Exception exception)
            {
                DisplayAlert("Error"
                           , exception.ToString()
                           , "OK");
            }
        }
        
        private async void AddMoodButton_OnClicked(object    sender
                                                 , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddMoodView));
        }

        private async void AddJournalTypeButton_OnClicked(object    sender
                                                        , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddJournalTypeView));
        }

        private void ListView_SwipeJournalTypeEnded(object              sender
                                                  , SwipeEndedEventArgs e)
        {
            SwipedJournalTypeItem = e.ItemIndex;
        }

        private void LeftImage_DeleteJournalType_BindingContextChanged(object    sender
                                                                     , EventArgs e)
        {
            if (sender is Image deleteImage)
            {
                (deleteImage.Parent as View)?.GestureRecognizers
                                             .Add(new TapGestureRecognizer
                                                  {
                                                      Command = new Command(DeleteJournalType)
                                                  });
            }
        }

        private void LeftImage_EditJournalType_BindingContextChanged(object    sender
                                                                   , EventArgs e)
        {
            if (sender is Image editImage)
            {
                (editImage.Parent as View)?.GestureRecognizers
                                           .Add(new TapGestureRecognizer
                                                {
                                                    Command = new Command(EditJournalType)
                                                });
            }
        }
        
        private async void EditJournalType(object obj)
        {
            JournalTypeToEdit = ViewModel.JournalTypeViewModel.GetJournalTypeToEdit(SwipedJournalTypeItem);

            await PageNavigation.NavigateTo(nameof(EditJournalTypePopUp)
                                          , nameof(EditJournalTypePopUp.JournalTypeId)
                                          , JournalTypeToEdit.Id.ToString());
            
            if (PageCommunication.Instance.IntegerValue != 0)
            {
                ViewModel.JournalTypeViewModel.RefreshListOfJournalTypes();
                JournalTypesListView.ItemsSource = ViewModel.JournalTypeViewModel.ObservableJournalTypes;
                
                PageCommunication.Instance.Clear();
            }
        }

        /// <summary>
        /// This will be used to pick from any stored backed up database, when there is an option to backup multiple database.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private async Task<string> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);
                
                return result.FileName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Cancelled or Error"
                                 , ex.Message
                                 , "OK");
            }
    
            return null;
        }

        private async void BackupDatabase()
        {
            var destinationPath = BackupViewModel.BackupDataFromSource();

            await DisplayAlert("Backup Complete"
                             , GetBackedUpMessageText(destinationPath)
                             , "OK");

            // var backedUpFileNameAndPath = BackupViewModel.Backup();
            //
            // await DisplayAlert("DB Backed Up"
            //                  , GetBackedUpMessageText(backedUpFileNameAndPath)
            //                  , "OK");

        }

        private static string GetBackedUpMessageText(string backedUpFileNameAndPath)
        {
            var message = new StringBuilder();
            message.AppendLine("The database was backed up to:");
            message.AppendLine(backedUpFileNameAndPath);
            
            return message.ToString();
        }

        private async void RestoreDatabase()
        {
            var restoreOk = await DisplayAlert("Database Restore"
                                             , GetRestoredMessageText()
                                             , "OK"
                                             , "Cancel");

            if ( ! restoreOk)
                return;

            // var fileToRestore = await PickAndShow(PickOptions.Default);
            //
            // if ( ! await GetPermissions())
            // {
            //     return;
            // }

            try
            {
                // BackupViewModel.Restore(fileToRestore);
                BackupViewModel.Restore();
            }
            catch (UnauthorizedAccessException  accessException)
            {
                await DisplayAlert("Error"
                                 , accessException.Message
                                 , "OK");
            }
        }

        private string GetRestoredMessageText()
        {
            var message = new StringBuilder();
            message.AppendLine($"The restore process will remove all data in your working database and replace it the backup.");
            message.AppendLine("This application only knows about two locations then dealing with the databases:");
            message.AppendLine("  * The working database");
            message.AppendLine("  * THe backed up database");
            message.AppendLine("There is no way to undo this process.");
            message.AppendLine("Tap Cancel to cancel the restore process");

            return message.ToString();
        }

        private async Task<bool> GetPermissions()
        {
            var status = await CheckAndRequestPermissionAsync(new Permissions.StorageRead());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return false;
            }
            
            status = await CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
            return status == PermissionStatus.Granted;
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
        where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        private void BackUpDbButton_OnClicked(object    sender
                                            , EventArgs e)
        {
            BackupDatabase();
        }

        private void RestoreDbButton_OnClicked(object    sender
                                             , EventArgs e)
        {
            RestoreDatabase();
        }

        private void FixJournalTypes_OnClicked(object    sender
                                             , EventArgs e)
        {
            ViewModel.FixJournalTypes();
        }

        private void CleanupMoods_OnClicked(object    sender
                                          , EventArgs e)
        {
            ViewModel.CleanupMoods();
        }
    }
}