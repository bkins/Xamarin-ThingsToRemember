using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Avails.D_Flat;
using Avails.Xamarin;
using MediaManager;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ThingsToRemember.ViewModels;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views;

[QueryProperty(nameof(EntryId), nameof(EntryId))]
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EntryMediaPage : ContentPage, IQueryAttributable
{
    public string EntryId       { get ; set ; }
    public bool   LeftToGetData { get;  set; }

    private          EntryViewModel _entryViewModel;
    private readonly string         _personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

    public EntryMediaPage()
    {
        InitializeComponent();
        
    }

    public void ApplyQueryAttributes(IDictionary<string, string> query)
    {
        EntryId = HttpUtility.UrlDecode(query[nameof(EntryId)]);

        if (EntryId.IsNullEmptyOrWhitespace())
        {
            Logger.WriteLineToToastForced("Could not retrieve Entry.  Please try again.", Category.Error);
            return;
        }
        
        LoadEntry(int.Parse(EntryId));
    }

    private void LoadEntry(int entryIntId)
    {
        _entryViewModel = new EntryViewModel(entryIntId);

        Title   = _entryViewModel.Title;
        EntryId = _entryViewModel.Entry.Id.ToString();
            
        BindingContext = _entryViewModel;

        //Load image, if one exists
         if (_entryViewModel.Entry.Image.Length > 0)
         {
             //var imageStream = new MemoryStream(_entryViewModel.Entry.Image);
             ImageFromCamera.Source = ImageSource.FromStream(() => new MemoryStream(_entryViewModel.Entry.Image));
         }
        
         ImageFromCamera.IsVisible = _entryViewModel.Entry.Image.Length > 0;
        
         if (_entryViewModel.Entry.VideoFileName.HasValue())
         {
             VideoFromCamera.Source = MediaSource.FromFile(_entryViewModel.Entry.VideoFileName);
        
             //VideoMediaElement.IsVisible = true;
             //var videoStream = new MemoryStream(_entryViewModel.Entry.Video);
             //VideoMediaElement.Source = videoStream;
        
             //VideoFromCamera.Source = MediaManager.CrossMediaManager
         }
        
         VideoFromCamera.IsVisible = _entryViewModel.Entry.VideoFileName.HasValue();
         PlayLabel.IsVisible       = _entryViewModel.Entry.VideoFileName.HasValue();
         StopLabel.IsVisible       = _entryViewModel.Entry.VideoFileName.HasValue();
        
            
        ShowHideMediaGrid();
    }

    private void ShowHideMediaGrid()
    {
        var entryHasAnyMedia = _entryViewModel.Entry.ImageFileName.HasValue()
                            || _entryViewModel.Entry.VideoFileName.HasValue();
    
        PhotoVideoGrid.IsVisible = entryHasAnyMedia;
        
        //Dynamically set the size of the grid that holds the media
        // if (entryHasAnyMedia)
        // {
        //     MainGrid.RowDefinitions[1] = new RowDefinition
        //                                  {
        //                                      Height = new GridLength(20
        //                                                            , GridUnitType.Star)
        //                                  };
        //
        //     MainGrid.RowDefinitions[4] = new RowDefinition
        //                                  {
        //                                      Height = new GridLength(30
        //                                                            , GridUnitType.Star)
        //                                  };
        // }
        // else
        // {
        //     MainGrid.RowDefinitions[1] = new RowDefinition
        //                                  {
        //                                      Height = GridLength.Auto
        //                                  };
        //     
        //     MainGrid.RowDefinitions[4] = new RowDefinition
        //                                  {
        //                                      Height = GridLength.Star
        //                                  };
        // }
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

            using var memory = new MemoryStream();

            var stream = photo.GetStream();
            await stream.CopyToAsync(memory);
            _entryViewModel.Entry.Image = memory.ToArray();

            ImageFromCamera.IsVisible = true;

            _entryViewModel.Entry.ImageFileName = $"Entry{_entryViewModel.Entry.Id}.jpg";
            LeftToGetData                       = false;
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

            ShowHideMediaGrid();
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

    private async Task<bool> ReadyToTakePicture()
    {
        //Open camera
        if ( ! CrossMedia.Current.IsCameraAvailable
         ||  ! CrossMedia.Current.IsTakePhotoSupported)
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
        //BENDO:  Should it support multiple images/vidoes? 
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

            VideoFromCamera.Source = ImageSource.FromStream(() => video.GetStream());

            using (var memory = new MemoryStream())
            {
                var stream = video.GetStream();
                await stream.CopyToAsync(memory);
                _entryViewModel.Entry.Video = memory.ToArray();

                //To save to DB
                //_entryViewModel.Entry.Video = memory.ToArray();

                var videoPath = Path.Combine(_personalFolder
                                           , $"{_entryViewModel.Entry.Title}_{_entryViewModel.Entry.Id}.mp4");

                _entryViewModel.Entry.VideoFileName = videoPath;

                File.WriteAllBytes(videoPath
                                 , memory.ToArray());

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

            ShowHideMediaGrid();
        }
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

    private void SaveButton_OnClicked(object    sender
                                    , EventArgs e)
    {
        _entryViewModel.Save();
    }
}