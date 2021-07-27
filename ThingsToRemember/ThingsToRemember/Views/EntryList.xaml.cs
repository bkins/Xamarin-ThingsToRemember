using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(JournalId), nameof(JournalId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryList : ContentPage
    {
        private string _journalId;
        public string JournalId
        {
            get => _journalId;
            set => SetCollectionViewItemSource(value);
        }

        private EntriesByJournalViewModel _entriesViewModel;

        public EntryList()
        {
            InitializeComponent();
        }

        private void SetCollectionViewItemSource(string value)
        {
            _journalId                 = value;
            _entriesViewModel          = new EntriesByJournalViewModel(int.Parse(JournalId));
            CollectionView.ItemsSource = _entriesViewModel.Entries;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

        }

        private async void OnSelectionChanged(object                    sender
                                            , SelectionChangedEventArgs e)
        {
            var entry = (Models.Entry)e.CurrentSelection?.FirstOrDefault();

            if (entry != null)
            {
                var path = $"{nameof(EntryPage)}?{nameof(EntryPage.EntryId)}={entry.Id}";
                await Shell.Current.GoToAsync(path);
            }
        }
    }
}