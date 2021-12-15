using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryListView : ContentPage, IQueryAttributable
    {
        public  string JournalId { get; set; }

        private EntriesByJournalViewModel _entriesViewModel;

        public EntryListView()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            JournalId = HttpUtility.UrlDecode(query[nameof(JournalId)]);
            
            _entriesViewModel    = new EntriesByJournalViewModel(int.Parse(JournalId));
            Title                = _entriesViewModel.Title;
            ListView.ItemsSource = _entriesViewModel.ObservableListOfEntries;
            
            var testEntries = _entriesViewModel.Entries;
            var testMood    = testEntries.FirstOrDefault(fields => fields.EntryMood != null)
                                        ?.EntryMood;

            //var mooodAsString = testMood.ToString();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            ListView.IsVisible        = true;
            EditEntryGrid.IsVisible = false;
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
            await PageNavigation.NavigateTo(nameof(EntryPage)
                                          , nameof(EntryPage.EntryId)
                                          , "0"
                                          , nameof(EntryPage.JournalId)
                                          , JournalId);
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