using System;
using System.Linq;
using System.Text;
using ApplicationExceptions;
using Avails.D_Flat;
using Avails.Xamarin;
using Avails.Xamarin.Controls;
using Syncfusion.ListView.XForms;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        private JournalsViewModel _journalsViewModel;
        
        public int                  SwipedItem           { get; set; }
        public Journal              JournalToEdit        { get; set; }
        public JournalTypeViewModel JournalTypeViewModel { get; private set; }
        public DateTime             NowForTtr            { get; set; }

        public InitialPage()
        {
            InitializeComponent();
            JournalTypeViewModel = new JournalTypeViewModel();
            
            JournalTypePicker.ItemsSource = JournalTypeViewModel.JournalTypes;

#if DEBUG
            NowDateTimeGrid.IsVisible = true;
#endif
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                RefreshPage(DateTime.Now);
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

        private void RefreshPage(DateTime dateTimeNow)
        {
            _journalsViewModel   = new JournalsViewModel();
            Title                = _journalsViewModel.Title;
            ListView.ItemsSource = _journalsViewModel.ObservableListOfJournals;

            ListView.IsVisible        = true;
            EditJournalGrid.IsVisible = false;

            JournalTypeViewModel.RefreshListOfJournalTypes();

            SetInitialImageAndTextVisibility();

            TtrToolbarItem.IsEnabled = _journalsViewModel.AnyWithTtr(dateTimeNow);
        }

        private void SetInitialImageAndTextVisibility(bool refreshJournalList = false)
        {
            if (refreshJournalList)
            {
                _journalsViewModel.RefreshListOfJournals();
            }

            InitialImageButton.IsVisible    = ! _journalsViewModel.ObservableListOfJournals.Any();
            ClickHereToBeginLabel.IsVisible = InitialImageButton.IsVisible;
            JournalColumnHeaders.IsVisible  = ! InitialImageButton.IsVisible;
        }
        
        private async void AddJournal_Clicked(object    sender
                                              , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddJournalView));
        }
        
        private async void OnSelectionChanged(object                        sender
                                      , ItemSelectionChangedEventArgs e)
        {
            var journalToOpen = (Journal)e.AddedItems[0];

            await PageNavigation.NavigateTo(nameof(EntryListView)
                                          , nameof(EntryListView.JournalId)
                                          , journalToOpen.Id.ToString());
        }

        private void ListView_SwipeEnded(object              sender
                                       , SwipeEndedEventArgs e)
        {
            SwipedItem = e.ItemIndex;
        }

        private void LeftImage_Delete_BindingContextChanged(object    sender
                                                          , EventArgs e)
        {
            if (sender is Image deleteImage)
            {
                (deleteImage.Parent as View)?.GestureRecognizers
                                             .Add(new TapGestureRecognizer
                                                  {
                                                      Command = new Command(Delete)
                                                  });
            }
        }
        
        private void LeftImage_Edit_BindingContextChanged(object    sender
                                                        , EventArgs e)
        {
            if (sender is Image editImage)
            {
                (editImage.Parent as View)?.GestureRecognizers
                                           .Add(new TapGestureRecognizer
                                                {
                                                    Command = new Command(Edit)
                                                });
            }
        }

        private void ToggleEditView()
        {
            ListView.IsVisible              = ! ListView.IsVisible;
            EditJournalGrid.IsVisible       = ! EditJournalGrid.IsVisible;
            AddJournalToolbarItem.IsEnabled = ! AddJournalToolbarItem.IsEnabled;
            JournalColumnHeaders.IsVisible  = ! JournalColumnHeaders.IsVisible;
        }

        private void Edit(object obj)
        {
            JournalToEdit = _journalsViewModel.GetJournalToEdit(SwipedItem);
            SetEditFields();

            EditJournalGrid.IsVisible       = true;
            ListView.IsVisible              = false;
            JournalColumnHeaders.IsVisible  = false;
            AddJournalToolbarItem.IsEnabled = false;
            
            ListView.ResetSwipe();
        }

        private void SetEditFields()
        {
            JournalTitleEntry.Text = JournalToEdit.Title;
            JournalTypeLabel.Text  = JournalToEdit.JournalType?.Title;
        }

        private async void Delete()
        {
            var journalToDelete = _journalsViewModel.GetJournal(SwipedItem);

            var userChoice = await DisplayAlert("Are you sure?"
                                              , GetAreYouSureYouWouldLikeToProceedMessage(journalToDelete)
                                              , "Yes"
                                              , "No");

            if (userChoice)
            {
                var itemDeleted = DeleteSwipedItem(journalToDelete);

                ListView.ItemsSource = _journalsViewModel.ObservableListOfJournals;
                SetInitialImageAndTextVisibility();

                Logger.WriteLine($"Deleted Journal: {itemDeleted} deleted."
                               , Category.Information);

            }
            
            ListView.ResetSwipe();
        }

        private static string GetAreYouSureYouWouldLikeToProceedMessage(Journal journalToDelete)
        {
            var message = new StringBuilder();
            message.AppendLine($"You are about to delete the journal: '{journalToDelete.Title}'.");
            message.AppendLine("Are you sure you would like to proceed?");

            return message.ToString();
        }

        private string DeleteSwipedItem(Journal journalToDelete)
        {
            var itemDeleted = _journalsViewModel.Delete(SwipedItem
                                                      , journalToDelete);

            if (itemDeleted.IsNullEmptyOrWhitespace())
            {
                Logger.WriteLine("Journal could not be deleted.  Please try again."
                               , Category.Warning);
            }

            return itemDeleted;
        }

        private void DoneEditingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            JournalToEdit.Title = JournalTitleEntry.Text;

            SaveJournalToEdit();

            ToggleEditView();
            
            JournalTypePickerVisible(false);

            _journalsViewModel.RefreshListOfJournals();

            ListView.ItemsSource = _journalsViewModel.ObservableListOfJournals;
        }

        private void SaveJournalToEdit()
        {
            try
            {
                _journalsViewModel.Save(JournalToEdit);
            }
            catch (Exception exception)
            {
                DisplayAlert("Error"
                           , exception.ToString()
                           , "OK");
            }
        }

        private void PickJournalTypeLabel_OnTapped(object    sender
                                                 , EventArgs e)
        {
            JournalTypePickerVisible(true);
        }

        private void JournalTypePicker_OnOkButtonClicked(object                                               sender
                                                       , Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            var selectedTypeTitle = JournalTypePicker.SelectedItem.ToString();

            JournalToEdit.JournalType = JournalTypeViewModel.GetJournalType(selectedTypeTitle);
            
            JournalTypeLabel.Text = selectedTypeTitle;

            JournalTypePickerVisible(false);
        }

        private void JournalTypePickerVisible(bool isVisible)
        {
            JournalTypePicker.IsVisible = isVisible;
            DoneEditingButton.IsEnabled = ! isVisible;
        }

        private void JournalTypePicker_OnCancelButtonClicked(object                                               sender
                                                           , Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            JournalTypePickerVisible(false);
        }

        private async void ConfigurationToolbarItem_OnClicked(object    sender
                                                      , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(ConfigurationView));
        }
        
        private async void ClickHereToBegin_OnTapped(object    sender
                                                  , EventArgs e)
        {
            if ( ! JournalTypeViewModel.ObservableJournalTypes.Any())
            {
                await DisplayAlert("Add Journal Type"
                                 , "Before you can add a Journal you must enter a/some Journal Types.  Please do that now."
                                 , "OK");

                await PageNavigation.NavigateTo(nameof(ConfigurationView));
            }
            else
            {
                await PageNavigation.NavigateTo(nameof(AddJournalView));
            }
        }

        private async void TtrToolbarItem_OnClicked(object    sender
                                            , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(EntryListView)
                                          , nameof(EntryListView.ShowTtr)
                                          , "YES"
                                          , nameof(EntryListView.DateTimeNow)
                                          , NowDatePicker.Date.ToShortDateString());
        }

        private void DeleteImage_OnTapped(object    sender
                                                 , EventArgs e)
        {
            Delete();
        }

        private void ApplyTimeHopButton_OnClicked(object    sender
                                                , EventArgs e)
        {
            TtrToolbarItem.IsEnabled = _journalsViewModel.AnyWithTtr(NowDatePicker.Date);
            
        }

        private void TtRColumnHeader_OnTapped(object    sender
                                                 , EventArgs e)
        {
            Logger.WriteLineToToastForced("Number of entries that occurred on this day some amount of years ago for each Journal."
                                        , Category.Information);
        }

        private void EntriesColumnHeader_OnTapped(object    sender
                                                 , EventArgs e)
        {
            Logger.WriteLineToToastForced("Number of entries for each Journal.", Category.Information);
        }
    }
}