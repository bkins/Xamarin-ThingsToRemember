using System;
using System.Threading.Tasks;
using Avails.Xamarin;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SelectionChangedEventArgs = Syncfusion.SfPicker.XForms.SelectionChangedEventArgs;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(JournalId), nameof(JournalId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddJournalView : ContentPage
    {
        private string _journalId;
        public string JournalId
        {
            get => _journalId;
            set => SetCollectionViewItemSource(value);
        }
        public JournalViewModel     JournalViewModel    { get; set; }
        public JournalTypeViewModel JournalTypeViewModel { get; set; }
        public JournalType          SelectedJournalType { get; set; }

        public AddJournalView()
        {
            InitializeComponent();
            JournalTypeViewModel = new JournalTypeViewModel();
            
            JournalTypePicker.ItemsSource = JournalTypeViewModel.JournalTypes;
        }
        
        private void SetCollectionViewItemSource(string value)
        {
            _journalId                = value;
            JournalViewModel          = new JournalViewModel(int.Parse(JournalId));
            Title                     = JournalViewModel.Journal.Title;
            PickJournalTypeLabel.Text = JournalViewModel.Journal.JournalType.Title;
        }

        private void PickJournalTypeLabel_OnTapped(object    sender
                                                 , EventArgs e)
        {
            ToggleJournalTypePickerVisibility();
        }

        private void ToggleJournalTypePickerVisibility()
        {
            PickJournalTypeLabel.IsVisible = ! PickJournalTypeLabel.IsVisible;
            JournalTypePicker.IsVisible    = ! JournalTypePicker.IsVisible;
        }

        private void JournalTypePicker_OnOkButtonClicked(object                    sender
                                                       , SelectionChangedEventArgs e)
        {
            ToggleJournalTypePickerVisibility();
            SelectedJournalType = (JournalType)JournalTypePicker.SelectedItem;
            
            Device.BeginInvokeOnMainThread(() =>
            {
                PickJournalTypeLabel.Text = SelectedJournalType?.Title;
            });
            
        }
        
        private void JournalTypePicker_OnCancelButtonClicked(object                    sender
                                                           , SelectionChangedEventArgs e)
        {
            ToggleJournalTypePickerVisibility();
        }

        private async void SaveButton_OnClicked(object    sender
                                              , EventArgs e)
        {
            await SaveNewJournal();
            await PageNavigation.NavigateBackwards();
        }

        private async Task SaveNewJournal()
        {
            JournalViewModel = new JournalViewModel(JournalTitleEntry.Text
                                                  , PickJournalTypeLabel.Text);
            try
            {
                JournalViewModel.Save();
            }
            catch (DuplicateWaitObjectException)
            {
                await DisplayAlert("Journal Exists"
                                 , "There is already a journal with the same name and type.  Please either rename the journal or change the type."
                                 , "OK");
            }
            catch (Exception exception)
            {
                await DisplayAlert("Error"
                                 , exception.Message
                                 , "OK");
            }
        }
    }
}