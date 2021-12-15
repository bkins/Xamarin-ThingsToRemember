using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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