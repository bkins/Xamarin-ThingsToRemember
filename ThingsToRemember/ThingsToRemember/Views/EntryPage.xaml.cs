using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using ApplicationExceptions;
using Avails.D_Flat;
using Avails.Xamarin;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Xamarin.Forms.Entry;
using SelectionChangedEventArgs = Syncfusion.SfPicker.XForms.SelectionChangedEventArgs;

namespace ThingsToRemember.Views
{
    [QueryProperty(nameof(EntryId), nameof(EntryId))]
    [QueryProperty(nameof(JournalId), nameof(JournalId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage, INotifyPropertyChanged,  IQueryAttributable
    {
        public string EntryId { get; set; }

        public  string        JournalId     { get; set; }
        private  MoodViewModel _moodViewModel { get; set; }

        private EntryViewModel _entryViewModel;

        public EntryPage()
        {
            InitializeComponent();
            BindingContext = new Entry();
            _moodViewModel = new MoodViewModel();

            RefreshMoodPicker();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            try
            {
                EntryId   = HttpUtility.UrlDecode(query[nameof(EntryId)]);
                JournalId = HttpUtility.UrlDecode(query[nameof(JournalId)]);

                _entryViewModel       = new EntryViewModel(int.Parse(EntryId));
                Title                 = _entryViewModel.Title;

                SetDateTimePickers();

                if (_entryViewModel.Entry.EntryMood != null)
                {
                    MoodLabel.Text = _entryViewModel.Entry.EntryMood.ToStringWithText();
                }
                
                BindingContext  = _entryViewModel;
            }
            catch (Exception e)
            {
                var messsage = e.Message; 
            }
            

        }

        private void RefreshMoodPicker()
        {
            MoodPicker.ItemsSource = _moodViewModel.AllMoodsForPicker;
        }

        private void SetDateTimePickers()
        {
            if (EntryId == "0")
            {
                CreateDatePicker.Date = DateTime.Now;
                CreateTimePicker.Time = DateTime.Now.TimeOfDay;
            }
            else
            {
                CreateDatePicker.Date = _entryViewModel.CreateDateTime.Date;

                CreateTimePicker.Time = new TimeSpan(_entryViewModel.CreateDateTime.Hour
                                                   , _entryViewModel.CreateDateTime.Minute
                                                   , 0);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RefreshMoodPicker();    
        }

        private async void OnSaveButtonClicked(object    sender
                                       , EventArgs e)
        {
            var theJournalId = int.Parse(JournalId);

            try
            {
                if (EntryId == "0")
                {
                    SaveNewEntry(theJournalId);
                }
                else
                {
                    UpdateExistingEntry(theJournalId);
                }
                
            }
            catch (Exception exception)
            {
                await DisplayAlert("Error"
                                 , exception.Message
                                 , "OK");
            }
            
            await PageNavigation.NavigateBackwards();
        }

        private void UpdateExistingEntry(int theJournalId)
        {
            _entryViewModel.Entry.Title          = TitleEditor.Text;
            _entryViewModel.Entry.Text           = TextEditor.Text;
            _entryViewModel.Entry.CreateDateTime = DateTime.Parse($"{CreateDatePicker.Date.ToShortDateString()} {CreateTimePicker.Time.ToShortForm()}");

            if (_entryViewModel.Entry.EntryMood       != null
             && _entryViewModel.Entry.EntryMood.Title != MoodLabel.Text.Split(' ')[0])
            {
                var moodTitle = MoodLabel.Text.Split(' ')[0];
                var newMood   = _entryViewModel.FindMood(moodTitle);
                _entryViewModel.Entry.EntryMood = newMood;
            }

            _entryViewModel.Save(theJournalId);
        }

        private void SaveNewEntry(int theJournalId)
        {
            var newEntry = new Models.Entry
                           {
                               Title          = TitleEditor.Text
                             , Text           = TextEditor.Text
                             , CreateDateTime = DateTime.Parse($"{CreateDatePicker.Date.ToShortDateString()} {CreateTimePicker.Time.ToShortForm()}")
                             , EntryMood = new Mood
                                           {
                                               Emoji = MoodLabel.Text.Split(' ')[1]
                                             , Title = MoodLabel.Text.Split(' ')[0]
                                           }
                           };

            _entryViewModel.Save(newEntry
                               , theJournalId);
        }

        private async void OnDeleteButtonClicked(object    sender
                                               , EventArgs e)
        {
            if (EntryId != "0")
            {
                _entryViewModel.Delete();
            }

            await PageNavigation.NavigateBackwards();
        }

        private void ShowMoodPicker(bool show)
        {
            TitleEditor.IsVisible         = ! show;
            TextEditor.IsVisible          = ! show;
            //CreateDateLabel.IsVisible = ! show;
            SaveButton.IsVisible          = ! show;
            DeleteButton.IsVisible        = ! show;

            MoodPicker.IsVisible           = show;
        }

        private void ShowCreateDateTimePicker(bool show)
        {
            TitleEditor.IsVisible  = ! show;
            TextEditor.IsVisible   = ! show;
            MoodLabel.IsVisible    = ! show;
            SaveButton.IsVisible   = ! show;
            DeleteButton.IsVisible = ! show;
            
        }
        private void MoodLabel_OnTapped(object    sender
                                      , EventArgs e)
        {
            ShowMoodPicker(true);
        }

        private async void MoodPicker_OnOkButtonClicked(object                    sender
                                                , SelectionChangedEventArgs e)
        {
            var  selectMood  = (string)MoodPicker.SelectedItem;

            if (selectMood == "<Add New>")
            {
                await PageNavigation.NavigateTo(nameof(AddMoodView));
            }
            else
            {
                var moodViewModel = new MoodViewModel(selectMood.Split(' ')[0]);
                _entryViewModel.Entry.EntryMood = moodViewModel.Mood;
            }

            MoodLabel.Text = _entryViewModel.Entry.EntryMood.ToStringWithText();

            ShowMoodPicker(false);
        }
        
        private void MoodPicker_OnCancelButtonClicked(object                    sender
                                                    , SelectionChangedEventArgs e)
        {
            ShowMoodPicker(false);
        }

        private void CreateDateTimeLabel_OnTapped(object    sender
                                                , EventArgs e)
        {
            ShowCreateDateTimePicker(true);
        }

        //private void CreateDatePicker_OnOkButtonClicked(object                    sender
        //                                                  , SelectionChangedEventArgs e)
        //{
        //    ShowCreateDateTimePicker(false);
        //}

        //private void CreateDateTimePicker_OnCancelButtonClicked(object                    sender
        //                                                      , SelectionChangedEventArgs e)
        //{
        //    ShowCreateDateTimePicker(false);
        //}

        private void CreateDatePicker_OnDateSelected(object               sender
                                                       , DateChangedEventArgs e)
        {
            
        }

        private void CreateTimePicker_OnPropertyChanged(object                   sender
                                                      , PropertyChangedEventArgs e)
        {
            
        }
    }
}