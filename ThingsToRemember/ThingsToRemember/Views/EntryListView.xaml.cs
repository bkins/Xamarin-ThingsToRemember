using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using Avails.D_Flat;
using Avails.Xamarin;
using Syncfusion.ListView.XForms;
using ThingsToRemember.Models;
using ThingsToRemember.Services;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = ThingsToRemember.Models.Entry;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;
using SwipeStartedEventArgs = Syncfusion.ListView.XForms.SwipeStartedEventArgs;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(JournalId), nameof(JournalId))]
    [QueryProperty(nameof(ShowTtr), nameof(ShowTtr))]
    [QueryProperty(nameof(DateTimeNow), nameof(DateTimeNow))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryListView : IQueryAttributable
    {
        public  string JournalId   { get; set; }
        public  string ShowTtr     { get; set; }
        public  string DateTimeNow { get; set; }
        private Entry  SwipedItem  { get; set; }

        private EntriesByJournalViewModel _entriesViewModel;
        private MoodViewModel             _moodViewModel;
        private JournalsViewModel         _journalsViewModel;
        
        public System.Collections.IList Journals { get; set; }
        
        public EntryListView()
        {
            InitializeComponent();

            DateTimeNow = DateTime.Now.ToShortDateString();
        }

        private string GetDecodedQueryValue(IDictionary<string, string> query, string nameofQueryKey)
        {
            try
            {
                return HttpUtility.UrlDecode(query[nameofQueryKey]);
            }
            catch (Exception )
            {
                return "0";
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            JournalId = GetDecodedQueryValue(query
                                           , nameof(JournalId));

            ShowTtr = GetDecodedQueryValue(query
                                         , nameof(ShowTtr));

            DateTimeNow = GetDecodedQueryValue(query
                                             , nameof(DateTimeNow));

            if (DateTimeNow == "0")
            {
                DateTimeNow = DateTime.Now.ToShortDateString();
            }

            _entriesViewModel = new EntriesByJournalViewModel(int.Parse(JournalId)
                                                            , DateTime.Parse(DateTimeNow)
                                                            , ShowTtr.IsTrue());

            Title = _entriesViewModel.Title;

            RefreshEntryListView();

            _journalsViewModel                      = new JournalsViewModel(JournalId);
            Journals                                = _journalsViewModel.JournalsToMoveTo;
            //SelectJournalToMoveToPicker.ItemsSource = new ObservableCollection<string>(Journals); //new[] { (IEnumerable)Journals };
            

            // var testEntries = _entriesViewModel.Entries;

            // var testMood    = testEntries.FirstOrDefault(fields => fields.EntryMood != null)
            //                             ?.EntryMood;

            //var moodAsString = testMood.ToString();
        }

        private void RefreshEntryListView()
        {
            // ListView.ItemsSource = ShowTtr.IsTrue() ?
            //     _entriesViewModel.ObservableListOfTtrEntries.OrderByDescending(fields => fields.CreateDateTime) :
            //     _entriesViewModel.ObservableListOfEntries.OrderByDescending(fields => fields.CreateDateTime);

            ListView.ItemsSource = _entriesViewModel.GetObservableEntries(ShowTtr.IsTrue());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ListView.IsVisible      = true;
            EditEntryGrid.IsVisible = false;

            _moodViewModel = new MoodViewModel();
        }

        private async void OnSelectionChanged(object                        sender
                                            , ItemSelectionChangedEventArgs itemSelectionChangedEventArgs)
        {
            var entry = (Entry)itemSelectionChangedEventArgs.AddedItems[0];

            if (entry != null)
            {
                await PageNavigation.NavigateTo(nameof(EntryPage)
                                              , nameof(EntryPage.EntryId)
                                              , entry.Id.ToString()
                                              , nameof(EntryPage.JournalId)
                                              , JournalId
                                              , nameof(EntryPage.ShowTtr)
                                              , ShowTtr);
            }
        }

        private async void AddEntryToolbarItem_OnClicked(object    sender
                                                 , EventArgs e)
        {
            if ( ! _moodViewModel.ObservableListOfMoods.Any())
            {
                await DisplayAlert("Let's Add Some Moods"
                                 , "Before you begin adding entries, let's start by adding some Moods!"
                                 , "OK");

                await PageNavigation.NavigateTo(nameof(ConfigurationView));
            }
            else
            {
                await PageNavigation.NavigateTo(nameof(EntryPage)
                                              , nameof(EntryPage.EntryId)
                                              , "0"
                                              , nameof(EntryPage.JournalId)
                                              , JournalId
                                              , nameof(EntryPage.ShowTtr)
                                              , ShowTtr);    
            }
        }

        private void ListView_OnSwipeStarted(object                sender
                                           , SwipeStartedEventArgs e)
        {
            Journals = _journalsViewModel.GetJournalsToMove();
            
            SelectJournalToMoveToPicker.ItemsSource = Journals;
            SelectJournalToMoveToPicker.IsVisible   = false;
        }
        
        private void ListView_SwipeEnded(object              sender
                                       , SwipeEndedEventArgs e)
        {

            SwipedItem = e.ItemData as Entry;
        }
        private async void Delete()
        {
            //var entryToDelete = _entriesViewModel.GetEntry(SwipedItem);
            var entryToDelete = SwipedItem;
            
            var userChoice = await DisplayAlert("Are you sure?"
                                              , GetAreYouSureYouWouldLikeToProceedMessage(entryToDelete)
                                              , "Yes"
                                              , "No");

            if (userChoice)
            {
                var itemDeleted = DeleteSwipedItem(entryToDelete);

                ListView.ItemsSource = _entriesViewModel.GetObservableEntries(ShowTtr.IsTrue());
                SetInitialImageAndTextVisibility();

                Logger.WriteLine($"Deleted Entry: {itemDeleted} deleted."
                               , Category.Information);

            }
            
            ListView.ResetSwipe();
        }

        private void SetInitialImageAndTextVisibility(bool refreshEntryList = false)
        {
            if (refreshEntryList)
            {
                _entriesViewModel.RefreshListOfEntries(DateTime.Parse(DateTimeNow));
            }
        }

        private string GetAreYouSureYouWouldLikeToProceedMessage(Entry entryToDelete)
        {
            var message = new StringBuilder();
            message.AppendLine($"You are about to delete the entry: '{entryToDelete.Title}'.");
            message.AppendLine("Are you sure you would like to proceed?");

            return message.ToString();
        }
        
        private string DeleteSwipedItem(Entry entryToDelete)
        {
            // var itemDeleted = _entriesViewModel.Delete(SwipedItem
            //                                          , entryToDelete);
            
            var itemDeleted = _entriesViewModel.Delete(entryToDelete, DateTime.Parse(DateTimeNow));
            
            if (itemDeleted.IsNullEmptyOrWhitespace())
            {
                Logger.WriteLine("Entry could not be deleted.  Please try again."
                               , Category.Warning);
            }

            return itemDeleted;
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
            
        }
        
        private void ToggleEditView()
        {
            ListView.IsVisible              = ! ListView.IsVisible;
            EditEntryGrid.IsVisible       = ! EditEntryGrid.IsVisible;
            AddEntryToolbarItem.IsEnabled = ! AddEntryToolbarItem.IsEnabled;
        }

        // private void Edit(object obj)
        // {
        //     ToggleEditView();
        // }
        private void DoneEditingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            
            ToggleEditView();

        }

        private async void SelectJournalToMoveToPicker_OnSelectedIndexChanged(object    sender
                                                                            , EventArgs e)
        {
            //BENDO: handle when cancel is pressed on picker: reset swipe
            SelectJournalToMoveToPicker.IsVisible = false;
            
            var confirm = await DisplayAlert("Journal Picked"
                             , $"Moving '{SwipedItem.Title}' to {SelectJournalToMoveToPicker.SelectedItem}"
                             , "Yes", "No");

            if (confirm)
            {
                if ( ! int.TryParse(SelectJournalToMoveToPicker.SelectedItem.ToString()
                                                            .Split('(')[1]
                                                            .Replace(")", "")
                                  , out var newJournalId))
                {
                    return;
                }
                
                _entriesViewModel.MoveEntry(SwipedItem
                                          , newJournalId);

                _entriesViewModel.RefreshListOfEntries(DateTime.Parse(DateTimeNow));
                RefreshEntryListView();
            }
        }

        private void MoveImage_OnClicked(object    sender
                                       , EventArgs e)
        {
            // SelectJournalToMoveToPicker.IsVisible = true;
            SelectJournalToMoveToPicker.Focus();
        }
    }
}