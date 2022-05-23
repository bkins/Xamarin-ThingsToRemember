using System;
using Avails.Xamarin;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMoodView : ContentPage
    {
        public MoodViewModel ViewModel { get; set; }

        public AddMoodView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
 
            ViewModel = new MoodViewModel();
        }

        private async void SaveButton_OnClicked(object    sender
                                        , EventArgs e)
        {
            if (MoodTitleEntry.Text.Contains(" "))
            {
                await DisplayAlert("Only One Word"
                                 , "Mood titles can only have one word."
                                 , "OK");
                return;
            }
            
            var mood = new Mood
                       {
                           Title = MoodTitleEntry.Text
                         , Emoji = MoodEmojiEntry.Text
                       };

            ViewModel.Save(mood);

            await PageNavigation.NavigateBackwards();
        }
    }
}