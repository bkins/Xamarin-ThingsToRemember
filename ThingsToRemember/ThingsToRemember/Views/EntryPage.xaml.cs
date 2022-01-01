using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Avails.D_Flat;
using Avails.Xamarin;
using MediaManager;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.CommunityToolkit.Core;
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
        public string EntryId       { get; set; }
        public bool   LeftToGetData { get; set; }


        public  string        JournalId     { get; set; }
        private  MoodViewModel _moodViewModel { get; set; }

        private EntryViewModel _entryViewModel;
        
        private readonly string _personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public EntryPage()
        {
            InitializeComponent();
            BindingContext = new Entry();
            _moodViewModel = new MoodViewModel();

            RefreshMoodPicker();

            //BENDO: Add ability to include a picture to an entry.
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

                //Load image, if one exists
                if (_entryViewModel.Entry.Image.Length > 0)
                {
                    ImageFromCamera.IsVisible = true;
                    //var imageStream = new MemoryStream(_entryViewModel.Entry.Image);
                    ImageFromCamera.Source = ImageSource.FromStream(() => new MemoryStream(_entryViewModel.Entry.Image));
                }

                if (_entryViewModel.Entry.VideoFileName.HasValue())
                {
                    
                    VideoFromCamera.Source = MediaSource.FromFile(_entryViewModel.Entry.VideoFileName);
                    //VideoMediaElement.IsVisible = true;
                    //var videoStream = new MemoryStream(_entryViewModel.Entry.Video);
                    //VideoMediaElement.Source = videoStream;

                    VideoFromCamera.IsVisible = true;
                    PlayLabel.IsVisible       = true;
                    StopLabel.IsVisible       = true;

                    //VideoFromCamera.Source = MediaManager.CrossMediaManager
                }
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
            //var newEntry = new Models.Entry
            //               {
            //                   Title          = TitleEditor.Text
            //                 , Text           = TextEditor.Text
            //                 , CreateDateTime = DateTime.Parse($"{CreateDatePicker.Date.ToShortDateString()} {CreateTimePicker.Time.ToShortForm()}")
            //                 , EntryMood = new Mood
            //                               {
            //                                   Emoji = MoodLabel.Text.Split(' ')[1]
            //                                 , Title = MoodLabel.Text.Split(' ')[0]
            //                               }
            //               };

            //_entryViewModel.Save(newEntry
            //                   , theJournalId);

            _entryViewModel.Entry.Title           = TitleEditor.Text;
            _entryViewModel.Entry.Text            = TextEditor.Text;
            _entryViewModel.Entry.CreateDateTime  = DateTime.Parse($"{CreateDatePicker.Date.ToShortDateString()} {CreateTimePicker.Time.ToShortForm()}");

            _entryViewModel.Entry.EntryMood = _moodViewModel.GetMood(MoodLabel.Text.Split(' ')[0]
                                                                   , MoodLabel.Text.Split(' ')[1]);
            //Photo and Video are added to the view model's entry when they are added to the page.

            _entryViewModel.Save(int.Parse(JournalId));
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
            //DeleteButton.IsVisible        = ! show;

            MoodPicker.IsVisible           = show;
        }

        private void ShowCreateDateTimePicker(bool show)
        {
            TitleEditor.IsVisible  = ! show;
            TextEditor.IsVisible   = ! show;
            MoodLabel.IsVisible    = ! show;
            SaveButton.IsVisible   = ! show;
            //DeleteButton.IsVisible = ! show;
            
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

            //if (selectMood == "<Add New>")
            //{
            //    await PageNavigation.NavigateTo(nameof(AddMoodView));
            //}
            //else
            //{
            //    var moodViewModel = new MoodViewModel(selectMood.Split(' ')[0]);
            //    _entryViewModel.Entry.EntryMood = moodViewModel.Mood;
            //}

            var moodViewModel = new MoodViewModel(selectMood.Split(' ')[0]);
            _entryViewModel.Entry.EntryMood = moodViewModel.Mood;

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

        private async void AddImageButton_OnClicked(object    sender
                                                  , EventArgs e)
        {
            //BENDO: Refactor this method
            AddImageButton.IsEnabled = false;

            if ( ! await ReadyToTakePicture())
            {
                AddImageButton.IsEnabled = true;
                return;
            }

            try
            {
                var photo = await GetImage();

                if (await WasFileReturnedFromCamera(photo))
                {
                    AddImageButton.IsEnabled = true;

                    return; 
                }

                ImageFromCamera.Source = ImageSource.FromStream(() => photo.GetStream());

                using (var memory = new MemoryStream())
                {
                    var stream = photo.GetStream();
                    await stream.CopyToAsync(memory);
                    _entryViewModel.Entry.Image = memory.ToArray();
                }

                ImageFromCamera.IsVisible = true;

                _entryViewModel.Entry.ImageFileName = $"Entry{_entryViewModel.Entry.Id}.jpg";
                LeftToGetData                 = false;
            }
            catch (ObjectDisposedException disposedException)
            {
                await DisplayAlert("ObjectDisposedException"
                                 , disposedException.Message
                                 , "OK");
            }
            catch (Exception exception)
            {
                await DisplayAlert("Exception"
                                 , exception.Message
                                 , "OK");
            }
            finally
            {
                AddImageButton.IsEnabled = true;
            }
        }
        
        private async Task<bool> WasFileReturnedFromCamera(MediaFile file)
        {
            if (file != null)
                return false;

            await DisplayAlert("No picture/video found"
                             , "Unable to get photo/video. Please try again"
                             , "OK");

            return true;

        }

        private async Task<MediaFile> GetImage()
        {
            LeftToGetData = true;
            var file = await CrossMedia.Current
                                       .TakePhotoAsync(new StoreCameraMediaOptions
                                                       {
                                                           PhotoSize = PhotoSize.Medium
                                                         , Directory = "Entry"
                                                         , Name      = $"Entry{_entryViewModel.Entry.Id}.jpg"
                                                       });
            return file;
        }
        
        private async Task<MediaFile> GetVideo()
        {
            LeftToGetData = true;
            
            var video = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions()
                                                                {
                                                                    Directory = "Entry"
                                                                  , Name      = $"EntryVideo{_entryViewModel.Entry.Id}.mp4"
                                                                });
            
            return video;
        }

        private async Task<bool> ReadyToTakePicture()
        {
            
            //Open camera
            if (! CrossMedia.Current.IsCameraAvailable
             || ! CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera"
                                 , "This feature will not work with your device."
                                 , "OK");

                return false;
            }

            return true;
        }

        private async void AddVideoButton_OnClicked(object    sender
                                            , EventArgs e)
        {
            //BENDO:  Need to decide if app will support an Image and a Video. Also, should it support multiple of each? 
            //BENDO: Refactor this method
            AddVideoButton.IsEnabled = false;

            if ( ! await ReadyToTakePicture())
            {
                AddImageButton.IsEnabled = true;
                return;
            }

            try
            {
                var video = await GetVideo();
                
                if (await WasFileReturnedFromCamera(video))
                {
                    AddImageButton.IsEnabled = true;

                    return; 
                }

                //VideoFromCamera.Source = ImageSource.FromStream(() => video.GetStream());

                using (var memory = new MemoryStream())
                {
                    var stream = video.GetStream();
                    await stream.CopyToAsync(memory);
                    
                    //To save to DB
                    //_entryViewModel.Entry.Video = memory.ToArray();

                    var videoPath = Path.Combine(_personalFolder
                                               , $"{_entryViewModel.Entry.Title}_{_entryViewModel.Entry.Id}.mp4");

                    _entryViewModel.Entry.VideoFileName = videoPath;

                    File.WriteAllBytes(videoPath, memory.ToArray());

                    VideoFromCamera.Source = MediaSource.FromFile(videoPath);
                }

                ImageFromCamera.IsVisible = true;
                LeftToGetData             = false;
            }
            catch (ObjectDisposedException disposedException)
            {
                await DisplayAlert("ObjectDisposedException"
                                 , disposedException.Message
                                 , "OK");
            }
            catch (Exception exception)
            {
                await DisplayAlert("Exception"
                                 , exception.Message
                                 , "OK");
            }
            finally
            {
                AddImageButton.IsEnabled = true;
            }
        }

        private void PlayLabel_OnTapped(object    sender
                                                 , EventArgs e)
        {
            CrossMediaManager.Current.Play(_entryViewModel.Entry.VideoFileName);
        }

        private void StopLabel_OnTapped(object    sender
                                      , EventArgs e)
        {
            CrossMediaManager.Current.Stop();
        }
    }
}