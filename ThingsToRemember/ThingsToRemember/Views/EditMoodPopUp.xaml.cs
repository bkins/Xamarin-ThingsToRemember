using System;
using System.Collections.Generic;
using System.Web;
using Avails.Xamarin;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(MoodId), nameof(MoodId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMoodPopUp : Rg.Plugins.Popup.Pages.PopupPage, IQueryAttributable
    {
        public string        MoodId     { get; set; }
        public MoodViewModel ViewModel  { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            MoodId = HttpUtility.UrlDecode(query[nameof(MoodId)]);
            ViewModel = new MoodViewModel(int.Parse(MoodId));

            MoodTitleEntry.Text = ViewModel.Mood.Title;
            MoodEmojiEntry.Text = ViewModel.Mood.Emoji;
        }

        public EditMoodPopUp()
        {
            InitializeComponent();
            
        }
        public EditMoodPopUp(string moodId)
        {
            InitializeComponent();

            MoodId = moodId;

            ViewModel = new MoodViewModel(int.Parse(MoodId));

            MoodTitleEntry.Text = ViewModel.Mood.Title;
            MoodEmojiEntry.Text = ViewModel.Mood.Emoji;
        }
        
        private async void EditMoodPopUp_OnBackgroundClicked(object    sender
                                                     , EventArgs e)
        {
            ViewModel.Mood.Title = MoodTitleEntry.Text;
            ViewModel.Mood.Emoji = MoodEmojiEntry.Text;

            ViewModel.Save();

            PageCommunication.Instance.IntegerValue = 1;
            await PageNavigation.NavigateTo(nameof(ConfigurationView));
        }

    }
}