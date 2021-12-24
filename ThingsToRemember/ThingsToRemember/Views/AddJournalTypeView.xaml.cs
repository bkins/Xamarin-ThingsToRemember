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
    public partial class AddJournalTypeView : ContentPage
    {
        public JournalTypeViewModel ViewModel { get; set; }

        public AddJournalTypeView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel = new JournalTypeViewModel();
        }

        private async void SaveButton_OnClicked(object    sender
                                        , EventArgs e)
        {
            var journalType = new JournalType
                              {
                                  Title = JournalTypeTitleEntry.Text
                              };

            ViewModel.Save(journalType);

            await PageNavigation.NavigateBackwards();
        }
    }
}