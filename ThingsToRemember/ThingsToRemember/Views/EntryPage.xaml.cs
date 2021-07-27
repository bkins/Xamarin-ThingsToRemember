using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(EntryId), nameof(EntryId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage, INotifyPropertyChanged
    {
        private string _entryId;

        public string EntryId
        {
            get => _entryId; 
            set => SetCollectionViewItemSource(value);
        }

        private EntryViewModel _entryViewModel;

        public EntryPage()
        {
            InitializeComponent();
            BindingContext = new Entry();
        }
        private void SetCollectionViewItemSource(string value)
        {
            _entryId        = value;
            _entryViewModel = new EntryViewModel(int.Parse(EntryId));
            BindingContext  = _entryViewModel;
        }

        private void OnSaveButtonClicked(object    sender
                                       , EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnDeleteButtonClicked(object    sender
                                         , EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}