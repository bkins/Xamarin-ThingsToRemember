using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationExceptions;
using Avails;
using Avails.D_Flat;
using Avails.Xamarin;
using Syncfusion.ListView.XForms;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = ThingsToRemember.Models.Entry;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        public  int               SwipedItem { get; set; }
        private JournalsViewModel _journalsViewModel;
        
        public Journal JournalToEdit { get; set; }
        public JournalTypeViewModel JournalTypeViewModel { get; private set; }

        //public string Title { get; set; }

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
                _journalsViewModel              = new JournalsViewModel();
                Title                           = _journalsViewModel.Title;
                ListView.ItemsSource            = _journalsViewModel.ObservableListOfJournals;
                ListView.IsVisible              = true;
                EditJournalGrid.IsVisible       = false;
                SetInitialImageAndTestVisibility();
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

        private void SetInitialImageAndTestVisibility()
        {
            InitialImageButton.IsVisible    = ! _journalsViewModel.Journals.Any();
            ClickHereToBeginLabel.IsVisible = InitialImageButton.IsVisible;
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
        }

        private void Edit(object obj)
        {
            JournalToEdit = _journalsViewModel.GetJournalToEdit(SwipedItem);
            SetEditFields();

            ToggleEditView();
        }

        private void SetEditFields()
        {
            JournalTitleEntry.Text = JournalToEdit.Title;
            JournalTypeLabel.Text  = JournalToEdit.JournalType?.Title;
        }

        private void Delete()
        {
            //I had a time when I clicked to delete a journal and all journals were deleted.  
            //I am not sure how that was possible, but
            //BENDO: Prevent deleting of multiple journal.  Maybe a confirm before deleting?

            var itemDeleted = _journalsViewModel.Delete(SwipedItem);

            if (itemDeleted.IsNullEmptyOrWhitespace())
            {
                Logger.WriteLine("Journal could not be deleted.  Please try again."
                               , Category.Warning);
            }

            ListView.ItemsSource = _journalsViewModel.ObservableListOfJournals;

            Logger.WriteLine($"Deleted Journal: {itemDeleted} deleted."
                           , Category.Information);

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

            JournalToEdit.JournalType = JournalTypeViewModel.FindJournalType(selectedTypeTitle);

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

        private async void InitialImageButton_OnClicked(object    sender
                                                , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddJournalView));
        }

        private async void ClickHereToBeginLabel_OnTapped(object    sender
                                                  , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddJournalView));
        }
    }
}