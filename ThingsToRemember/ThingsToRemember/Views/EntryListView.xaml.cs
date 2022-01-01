using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Avails.D_Flat;
using Avails.Xamarin;
using Syncfusion.ListView.XForms;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = ThingsToRemember.Models.Entry;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(JournalId), nameof(JournalId))]
    [QueryProperty(nameof(ShowTtr), nameof(ShowTtr))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryListView : ContentPage, IQueryAttributable
    {
        public  string                    JournalId { get; set; }
        public  string                    ShowTtr   { get; set; }

        private EntriesByJournalViewModel _entriesViewModel;
        private MoodViewModel             _moodViewModel;

        public EntryListView()
        {
            InitializeComponent();
        }

        public string GetDecodedQueryValue(IDictionary<string, string> query, string nameofQueryKey)
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

            _entriesViewModel    = new EntriesByJournalViewModel(int.Parse(JournalId));
            Title                = _entriesViewModel.Title;

            if (ShowTtr.IsTrue())
            {
                ListView.ItemsSource = _entriesViewModel.ObservableListOfTtrEntries;
            }
            else
            {
                ListView.ItemsSource = _entriesViewModel.ObservableListOfEntries;
            }
            
            var testEntries = _entriesViewModel.Entries;

            var testMood    = testEntries.FirstOrDefault(fields => fields.EntryMood != null)
                                        ?.EntryMood;

            //var moodAsString = testMood.ToString();
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
                                              , JournalId);
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
                                              , JournalId);    
            }
        }

        private void ListView_SwipeEnded(object              sender
                                       , SwipeEndedEventArgs e)
        {
            
        }

        private void LeftImage_Delete_BindingContextChanged(object    sender
                                                          , EventArgs e)
        {
            
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

        private void Edit(object obj)
        {
            ToggleEditView();
        }
        private void DoneEditingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            
            ToggleEditView();

        }
    }
}