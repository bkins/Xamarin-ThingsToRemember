using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationExceptions;
using Avails.D_Flat;
using Avails.Xamarin;
using Syncfusion.ListView.XForms;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SwipeEndedEventArgs = Syncfusion.ListView.XForms.SwipeEndedEventArgs;

namespace ThingsToRemember.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationView : ContentPage
    {
        public int  SwipedItem { get; set; }
        public Mood MoodToEdit { get; set; }

        private ConfigurationViewModel _viewModel    { get; set; }

        public ConfigurationView()
        {
            InitializeComponent();

            _viewModel = new ConfigurationViewModel();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                _viewModel             = new ConfigurationViewModel();
                Title                  = "Configuration";
                ListView.ItemsSource   = _viewModel.MoodViewModel.ObservableListOfMoods;
                ListView.IsVisible     = true;
                EditMoodGrid.IsVisible = false;
            }
            catch (DuplicateRecordException duplicateRecordException)
            {
                DisplayAlert("Duplicate Record Error"
                           , duplicateRecordException.Message
                           , "OK");
            }
            catch (Exception exception)
            {
                DisplayAlert("Error"
                           , exception.Message
                           , "OK");
            }
        }

        private void ClearDataButton_OnClicked(object    sender
                                             , EventArgs e)
        {
            _viewModel.ClearData();
        }
        
        private void ListView_SwipeEnded(object              sender
                                       , SwipeEndedEventArgs e)
        {
            SwipedItem = e.ItemIndex;
        }

        private void LeftImage_Delete_BindingContextChanged(object    sender
                                                          , EventArgs e)
        {
            if (sender is Image deleteImage)
            {
                (deleteImage.Parent as View)?.GestureRecognizers
                                             .Add(new TapGestureRecognizer
                                                  {
                                                      Command = new Command(Delete)
                                                  });
            }
        }
        
        private async void Delete()
        {
            var itemDeleted = _viewModel.MoodViewModel.Delete(SwipedItem);

            await DisplayAlert("Mood Deleted"
                             , itemDeleted
                             , "OK");

            ListView.ItemsSource = _viewModel.MoodViewModel.ObservableListOfMoods;
            
            ListView.ResetSwipe();
        }

        private void LeftImage_Edit_BindingContextChanged(object    sender
                                                        , EventArgs e)
        {
            if (sender is Image editImage)
            {
                (editImage.Parent as View)?.GestureRecognizers
                                           .Add(new TapGestureRecognizer
                                                {
                                                    Command = new Command(Edit)
                                                });
            }
        }
        
        private void Edit(object obj)
        {
            MoodToEdit = _viewModel.MoodViewModel.GetMoodToEdit(SwipedItem);
            SetEditFields();

            ToggleEditView();
        }
        
        private void SetEditFields()
        {
            MoodTitleEntry.Text = MoodToEdit.Title;
            MoodEmojiEntry.Text = MoodToEdit.Emoji;
        }
        
        private void ToggleEditView()
        {
            ListView.IsVisible           = ! ListView.IsVisible;
            EditMoodGrid.IsVisible       = ! EditMoodGrid.IsVisible;
            AddMoodToolbarItem.IsEnabled = ! AddMoodToolbarItem.IsEnabled;
        }

        private void DoneEditingButton_OnClicked(object    sender
                                               , EventArgs e)
        {
            MoodToEdit.Title = MoodTitleEntry.Text;
            MoodToEdit.Emoji = MoodEmojiEntry.Text;

            SaveMoodToEdit();

            ToggleEditView();
            
           // JournalTypePickerVisible(false);

            _viewModel.MoodViewModel.RefreshListOfMoods();
            ListView.ItemsSource = _viewModel.MoodViewModel.ObservableListOfMoods;
        }

        private void SaveMoodToEdit()
        {
            try
            {
                _viewModel.MoodViewModel.Save(MoodToEdit);
            }
            catch (Exception exception)
            {
                DisplayAlert("Error"
                           , exception.ToString()
                           , "OK");
            }
        }

        private async void AddMood_Clicked(object    sender
                                   , EventArgs e)
        {
            await PageNavigation.NavigateTo(nameof(AddMoodView));
        }
    }
}