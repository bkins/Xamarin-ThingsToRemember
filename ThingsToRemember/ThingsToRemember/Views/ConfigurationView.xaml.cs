using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApplicationExceptions;
using Avails.Xamarin;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;
using Android.App;
using Application = Android.App.Application;
using StringBuilder = System.Text.StringBuilder;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationView : ContentPage
    {
        public int  SwipedMoodItem        { get; set; }
        public int  SwipedJournalTypeItem { get; set; }

        public  Mood        MoodToEdit        { get; set; }
        public  JournalType JournalTypeToEdit { get; set; }

        private ConfigurationViewModel  _viewModel      { get; set; }
        private BackupDatabaseViewModel BackupViewModel { get; set; }

        public string BackupFolderPath => GetBackupDestinationPath();

        private static string GetBackupDestinationPath()
        {
            var backupFolder = Path.Combine("storage"
                                          , "emulated"
                                          , "0"
                                          , "Download"
                                          , "BackupDb");

            if ( ! Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            return backupFolder;

            //return Path.Combine(Application.Context
            //                               .GetExternalFilesDir()
            //                               .AbsolutePath
            //                               .Replace(AppInfo.PackageName
            //                                      , string.Empty
            //                                       ) ?? 
            //                    string.Empty
            //                  , "BackupDbs");
        }

        public ConfigurationView()
        {
            InitializeComponent();

            _viewModel = new ConfigurationViewModel();
            // /data/user/0/BackupDbs
            BackupViewModel = new BackupDatabaseViewModel(BackupFolderPath);
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Title = "Configuration";

                _viewModel             = new ConfigurationViewModel();
                
                MoodsListView.ItemsSource        = _viewModel.MoodViewModel.ObservableListOfMoods;
                JournalTypesListView.ItemsSource = _viewModel.JournalTypeViewModel.ObservableJournalTypes;
                
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
                _viewModel.ClearUserData();
            }
            else
            {
                //Phew! That was close!
            }
        }

        private async Task<bool> AskUserToClearUserData()
        {
            var question = new StringBuilder();
            question.AppendLine("You are about to clear all journals and their data.");
            question.AppendLine("This cannot be undone.");
            question.AppendLine("All data will be lost!");
            question.AppendLine("Would you like to continue?");

            var userWouldLikeToClear = await DisplayAlert("Clear All Journal Data!?"
                                                        , question.ToString()
                                                        , "Yes"
                                                        , "No");
            return userWouldLikeToClear;
        }
        
        private async Task<bool> AskUserToClearAppData()
        {
            var question = new StringBuilder();
            question.AppendLine("You are about to clear all application (ex. Moods) data.");
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
                _viewModel.ClearAppData();

                MoodsListView.ItemsSource        = _viewModel.MoodViewModel.ObservableListOfMoods;
                JournalTypesListView.ItemsSource = _viewModel.JournalTypeViewModel.ObservableJournalTypes;
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
            var itemDeleted = _viewModel.MoodViewModel.DeleteMood(SwipedMoodItem);

            await DisplayAlert("Mood Deleted"
                             , itemDeleted
                             , "OK");

            MoodsListView.ItemsSource = _viewModel.MoodViewModel.ObservableListOfMoods;
            
            MoodsListView.ResetSwipe();
        }
        
        private async void DeleteJournalType()
        {
            var itemDeleted = _viewModel.JournalTypeViewModel.Delete(SwipedJournalTypeItem);

            await DisplayAlert("Journal Type Deleted"
                             , itemDeleted
                             , "OK");

            MoodsListView.ItemsSource = _viewModel.JournalTypeViewModel.ObservableJournalTypes;
            
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
            MoodToEdit = _viewModel.MoodViewModel.GetMoodToEdit(SwipedMoodItem);

            await PageNavigation.NavigateTo(nameof(EditMoodPopUp)
                                          , nameof(EditMoodPopUp.MoodId)
                                          , MoodToEdit.Id.ToString());

            //await Navigation.PushPopupAsync(new EditMoodPopUp(MoodToEdit.Id.ToString()));

            if (PageCommunication.Instance.IntegerValue != 0)
            {
                _viewModel.MoodViewModel.RefreshListOfMoods();
                MoodsListView.ItemsSource = _viewModel.MoodViewModel.ObservableListOfMoods;
                
                PageCommunication.Instance.Clear();
            }

            //SetEditFields();

            //ToggleEditView();
        }
        
        private void SetEditFields()
        {
            MoodTitleEntry.Text = MoodToEdit.Title;
            MoodEmojiEntry.Text = MoodToEdit.Emoji;
        }
        
        private void ToggleEditView()
        {
            MoodsListView.IsVisible                  = ! MoodsListView.IsVisible;
            EditMoodGrid.IsVisible              = ! EditMoodGrid.IsVisible;
            //AddMoodToolbarItem.IsEnabled        = ! AddMoodToolbarItem.IsEnabled;
        }

        private void DoneEditingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            MoodToEdit.Title = MoodTitleEntry.Text;
            MoodToEdit.Emoji = MoodEmojiEntry.Text;

            SaveMoodToEdit();

            ToggleEditView();
            
           // JournalTypePickerVisible(false);

            _viewModel.MoodViewModel.RefreshListOfMoods();
            MoodsListView.ItemsSource = _viewModel.MoodViewModel.ObservableListOfMoods;
        }

        private void SaveMoodToEdit()
        {
            try
            {
                _viewModel.MoodViewModel.Save(MoodToEdit);
            }
            catch (Exception exception)
            {
                DisplayAlert("Error"
                           , exception.ToString()
                           , "OK");
            }
        }

        private async void AddMood_Clicked(object    sender
                                         , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddMoodView));
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
            JournalTypeToEdit = _viewModel.JournalTypeViewModel.GetJournalTypeToEdit(SwipedJournalTypeItem);

            await PageNavigation.NavigateTo(nameof(EditJournalTypePopUp)
                                          , nameof(EditJournalTypePopUp.JournalTypeId)
                                          , JournalTypeToEdit.Id.ToString());

            //await Navigation.PushPopupAsync(new EditJournalTypePopUp(JournalTypeToEdit.Id.ToString()));

            if (PageCommunication.Instance.IntegerValue != 0)
            {
                _viewModel.JournalTypeViewModel.RefreshListOfJournalTypes();
                JournalTypesListView.ItemsSource = _viewModel.JournalTypeViewModel.ObservableJournalTypes;
                
                PageCommunication.Instance.Clear();
            }
        }
        async Task<string> PickAndShow(PickOptions options)
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
            var backedUpFileNameAndPath = BackupViewModel.Backup();
            
            await DisplayAlert("DB Backed Up"
                             , GetBackedUpMessageText(backedUpFileNameAndPath)
                             , "OK");

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
                                             , "Canel");

            if (! restoreOk)
                return;

            var fileToRestore = await PickAndShow(PickOptions.Default);

            if ( ! await GetPermissions())
            {
                return;
            }

            try
            {
                BackupViewModel.Restore(fileToRestore);
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
            message.AppendLine($"The restore process will look in the folder {BackupFolderPath} for the database to restore from.");
            message.AppendLine("Select the version the database file to restore.");
            message.AppendLine("Tap Cancel to cnacel the restore process");

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
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return false;
            }

            return true;
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
    }
}