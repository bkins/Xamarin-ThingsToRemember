using System;
using System.Collections.Generic;
using System.Web;
using Avails.Xamarin;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(JournalTypeId), nameof(JournalTypeId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditJournalTypePopUp : Rg.Plugins.Popup.Pages.PopupPage, IQueryAttributable
    {
        public string               JournalTypeId { get; set; }
        public JournalTypeViewModel ViewModel     { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            JournalTypeId = HttpUtility.UrlDecode(query[nameof(JournalTypeId)]);
            
            ViewModel = new JournalTypeViewModel(int.Parse(JournalTypeId));

            JournalTypeTitleEntry.Text = ViewModel.JournalType.Title;
        }

        public EditJournalTypePopUp()
        {
            InitializeComponent();
        }

        public EditJournalTypePopUp(string journalTypeId)
        {
            InitializeComponent();

            JournalTypeId = journalTypeId;

            ViewModel = new JournalTypeViewModel(int.Parse(JournalTypeId));

            JournalTypeTitleEntry.Text = ViewModel.JournalType.Title;
        }
        
        private async void EditJournalTypePopUp_OnBackgroundClicked(object    sender
                                                            , EventArgs e)
        {
            ViewModel.JournalType.Title = JournalTypeTitleEntry.Text;

            ViewModel.Save();

            PageCommunication.Instance.IntegerValue = 1;

            await PageNavigation.NavigateTo(nameof(ConfigurationView));
        }
    }
}