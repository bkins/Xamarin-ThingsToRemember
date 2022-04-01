using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using Avails.Xamarin;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Xamarin.Forms.Entry;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(EntryId), nameof(EntryId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryTextPage : ContentPage, INotifyPropertyChanged,  IQueryAttributable
    {
        public string EntryId { get; set; }

        private EntryViewModel _entryViewModel;
        
        public EntryTextPage()
        {
            InitializeComponent();
            BindingContext = new Entry();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                EntryId   = HttpUtility.UrlDecode(query[nameof(EntryId)]);
                
                var entryIntId   = int.Parse(EntryId);

                LoadEntry(entryIntId);
            }
            catch (Exception e)
            {
                var messsage = e.Message; 
            }
        }

        private void LoadEntry(int entryIntId)
        {
            _entryViewModel = new EntryViewModel(entryIntId);
            Title           = _entryViewModel.Title;
            BindingContext  = _entryViewModel;
        }

        protected override bool OnBackButtonPressed()
        {

            // Device.BeginInvokeOnMainThread(async () =>
            // {
            //     if (await DisplayAlert("Save First?"
            //                          , "Would you like to save any changes before leaving?"
            //                          , "Yes", "No"))
            //     {
            //         await SaveEntry();
            //     }
            // });
            // 
            //         
            // PageNavigation.NavigateTo(nameof(EntryListView));
            // 
            //base.OnBackButtonPressed();
            PageNavigation.NavigateTo(nameof(EntryPage)
                                    , nameof(EntryPage.EntryId)
                                    , EntryId
                                    , nameof(EntryPage.JournalId)
                                    , _entryViewModel.Entry.JournalId.ToString()
                                    , nameof(EntryPage.ShowTtr)
                                    , PageCommunication.Instance.StringValue);
            return true;
        }
        
        private async Task SaveEntry()
        {
            var theJournalId = _entryViewModel.Entry.JournalId;
            
            try
            {
                UpdateExistingEntry(theJournalId);
                Logger.WriteLineToToastForced("Saved", Category.Information);
            }
            catch (Exception exception)
            {
                await DisplayAlert("Error"
                                 , exception.Message
                                 , "OK");
            }
        }

        private void UpdateExistingEntry(int theJournalId)
        {
            _entryViewModel.Entry.Text           = TextEditor.Text;
            
            _entryViewModel.Save(theJournalId);
        }
        
        private async void TapGestureRecognizer_OnTapped(object    sender
                                                       , EventArgs e)
        {
            await SaveEntry();
            //await PageNavigation.NavigateBackwards();
            await PageNavigation.NavigateTo(nameof(EntryPage)
                                          , nameof(EntryPage.EntryId)
                                          , EntryId
                                          , nameof(EntryPage.JournalId)
                                          , _entryViewModel.Entry.JournalId.ToString()
                                          , nameof(EntryPage.ShowTtr)
                                          , PageCommunication.Instance.StringValue);
        }
        
        
    }
}