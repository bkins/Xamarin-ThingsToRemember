using System;
using System.Linq;
using ApplicationExceptions;
using Avails.D_Flat;
using Avails.Xamarin;
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
        
        public InitialPage()
        {
            InitializeComponent();
            JournalTypeViewModel = new JournalTypeViewModel();
            
            JournalTypePicker.ItemsSource = JournalTypeViewModel.JournalTypes;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                _journalsViewModel        = new JournalsViewModel();
                Title                     = _journalsViewModel.Title;
                ListView.ItemsSource      = _journalsViewModel.ObservableListOfJournals;
                ListView.IsVisible        = true;
                EditJournalGrid.IsVisible = false;

                JournalTypeViewModel.RefreshListOfJournalTypes();

                SetInitialImageAndTextVisibility();
                
                TtrToolbarItem.IsEnabled      = _journalsViewModel.AnyWithTtr();
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

        private void SetInitialImageAndTextVisibility()
        {
            InitialImageButton.IsVisible    = ! _journalsViewModel.Journals.Any();
            ClickHereToBeginLabel.IsVisible = InitialImageButton.IsVisible;
            JournalColumnHeaders.IsVisible  = ! InitialImageButton.IsVisible;
        }

        private async void OnSelectionChanged(object                    sender
                                            , SelectionChangedEventArgs e)
        {
            var journal = (Journal) e.CurrentSelection?.FirstOrDefault();

            if (journal == null)
                return;

            //var path = $"{nameof(EntryListView)}?{nameof(EntryListView.JournalId)}={journal.Id}";
            //await Shell.Current.GoToAsync(path);

            await PageNavigation.NavigateTo(nameof(EntryListView)
                                          , nameof(EntryListView.JournalId)
                                          , journal.Id.ToString());
        }

        private async void AddJournal_Clicked(object    sender
                                              , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddJournalView));
        }

        private void EditImageButton_OnClicked(object    sender
                                             , EventArgs e)
        {
            var itemData         = (ImageButton)sender;
            //var commandParameter = itemData.CommandParameter;

            if (!(itemData.CommandParameter is JournalViewModel selectedJournal))
                return;

            //PageNavigation.NavigateTo(nameof(AddJournal), nameof(AddJournal.JournalId), (Journal) )
        }

        private void DeleteImageButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            
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

            //BENDO: In some cases, this is being called twice, causing the ToggleEditView to be called twice:
            //The first time shows the EditJournalGrid and hides/disables the appropriate elements, but then the second time it undoes that.
            //The above, explicit setting of the visibility/enable properties, works, but still need to prevent this method from being called twice.
            
            //ToggleEditView();
        }

        private void SetEditFields()
        {
            JournalTitleEntry.Text = JournalToEdit.Title;
            JournalTypeLabel.Text  = JournalToEdit.JournalType?.Title;
        }

        private async void Delete()
        {
            //BENDO: Prevent deleting of multiple journal.
            //I had a time when I clicked to delete a journal and all journals were deleted.  
            //I am not sure how that was possible, but
            //Maybe a confirm before deleting will be enough?
            //At least that will show if/when multiple are being deleted.
            
            var journalToDelete = _journalsViewModel.GetJournal(SwipedItem);

            var userChoice = await DisplayAlert("Are you sure?"
                                              , $"You are about to delete the journal: '{journalToDelete.Title}'. " 
                                              + "Are you sure you would like to proceed?"
                                              , "Yes"
                                              , "No");

            if (userChoice)
            {
                var itemDeleted = _journalsViewModel.Delete(SwipedItem, journalToDelete);

                if (itemDeleted.IsNullEmptyOrWhitespace())
                {
                    Logger.WriteLine("Journal could not be deleted.  Please try again."
                                   , Category.Warning);
                }

                ListView.ItemsSource = _journalsViewModel.ObservableListOfJournals;

                Logger.WriteLine($"Deleted Journal: {itemDeleted} deleted."
                               , Category.Information);

            }
            
            ListView.ResetSwipe();
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

            //JournalToEdit.JournalType = JournalTypeViewModel.JournalTypes
            //                                                .FirstOrDefault(fields => fields.Title == selectedTypeTitle);

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
                                          , "YES");
        }

        private void DeleteImage_OnTapped(object    sender
                                                 , EventArgs e)
        {
            Delete();
        }
    }
}