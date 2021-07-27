using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = ThingsToRemember.Models.Entry;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        private JournalsViewModel _journalsViewModel;
        public InitialPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _journalsViewModel         = new JournalsViewModel();
            CollectionView.ItemsSource = _journalsViewModel.Journals;
        }

        private async void OnSelectionChanged(object                    sender
                                            , SelectionChangedEventArgs e)
        {
            var journal = (Journal) e.CurrentSelection?.FirstOrDefault();

            if (journal != null)
            {
                var path = $"{nameof(EntryList)}?{nameof(EntryList.JournalId)}={journal.Id}";
                await Shell.Current.GoToAsync(path);
            }
        }
    }
}