using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Avails.D_Flat;
using Avails.Xamarin;
using MediaManager;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views;

[QueryProperty(nameof(EntryId), nameof(EntryId))]
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EntryMediaPage :  IQueryAttributable
{
    public string EntryId       { get ; set ; }
    public bool   LeftToGetData { get;  set; }

    private          EntryViewModel _entryViewModel;
    private          MediaViewModel _mediaViewModel;
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
        _mediaViewModel = new MediaViewModel(entryIntId);
        
        Title   = _mediaViewModel.MediaList.Count().ToString();
        EntryId = _entryViewModel.Entry.Id.ToString();

        BindingContext = _mediaViewModel;

        MediaListView.ItemsSource = _mediaViewModel.MediaList;
    }

    private void PlayLabel_OnTapped(object    sender
                                  , EventArgs e)
    {
        CrossMediaManager.Current.Play(_entryViewModel.Entry.VideoFileName);
        //CrossMediaManager.Current.Play(new MemoryStream(_entryViewModel.Entry.Video));
    }

    private void StopLabel_OnTapped(object    sender
                                  , EventArgs e)
    {
        CrossMediaManager.Current.Stop();
    }

    private async Task<bool> WasFileReturnedFromCamera(MediaFile file)
    {
        if (file != null)
            return true;

        await DisplayAlert("No picture/video found"
                         , "Unable to get photo/video. Please try again"
                         , "OK");

        return false;

    }
    private async Task<MediaFile> GetImage()
    {
        LeftToGetData = true;
        var image = await CrossMedia.Current
                                    .TakePhotoAsync(new StoreCameraMediaOptions
                                                    {
                                                        PhotoSize = PhotoSize.Medium
                                                      , Directory = "Entry"
                                                      , Name      = FormatFileName(_entryViewModel.Entry.Id
                                                                                 , MediaType.Image)
                                                    });
        return image;
    }

    private async Task<MediaFile> GetVideo()
    {
        LeftToGetData = true;
        var video = await CrossMedia.Current
                                    .TakeVideoAsync(new StoreVideoOptions()
                                                    {
                                                        Directory = "Entry"
                                                      , Name      = FormatFileName(_entryViewModel.Entry.Id
                                                                                 , MediaType.Video)
                                                    });
            
        return video;
    }

    private string FormatFileName(int entryId, MediaType type)
    {
        var extension = type switch
        {
            MediaType.Image => ".jpg"
          , MediaType.Video => ".mp4"
          , _ => string.Empty
        };

        return $"~id~Entry{type}{entryId}{extension}";
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

    private async Task<bool> ReadyToTakeVideo()
    {
        //Open camera
        if ( ! CrossMedia.Current.IsCameraAvailable
         ||  ! CrossMedia.Current.IsTakeVideoSupported)
        {
            await DisplayAlert("No Camera"
                             , "This feature will not work with your device."
                             , "OK");

            return false;
        }

        return true;
    }

    private async Task<bool> DeviceReadyToGetMedia(MediaType type)
    {
        return type switch
        {
            MediaType.Video => await ReadyToTakeVideo()
          , MediaType.Image => await ReadyToTakePicture()
          , _ => false
        };
    }
    
    private async Task<MediaFile> GetMedia(MediaType type)
    {
        return type switch
        {
            MediaType.Image => await GetImage()
          , MediaType.Video => await GetVideo()
          , _ => new MediaFile(string.Empty
                             , null)
        };
    }

    private async Task AddMedia(MediaType type)
    {
        var deviceReady = await DeviceReadyToGetMedia(type);
        if ( ! deviceReady )
        {
            return;
        }

        try
        {
            var media       = await GetMedia(type);
            var pathToMedia = media.Path;

            if ( ! await WasFileReturnedFromCamera(media))
            {
                return;
            }

            using var memory = new MemoryStream();
            
            var stream = media.GetStream();
            await stream.CopyToAsync(memory);

            var fileInfo    = new FileInfo(pathToMedia);
            var newFileName = fileInfo.Name;
            
            var mediaPath = Path.Combine(_personalFolder
                                       , newFileName); 

            _mediaViewModel.Save(type
                               , memory.ToArray()
                               , mediaPath );
            
            LeftToGetData = false;

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
    }

    private async Task AddImage()
    {
        if ( ! await ReadyToTakePicture())
        {
            return;
        }
        
        try
        {
            var photo       = await GetImage();
            var pathToMedia = photo.Path;
        
            if ( ! await WasFileReturnedFromCamera(photo))
            {
                return;
            }
            using var memory = new MemoryStream();

            var stream = photo.GetStream();
            await stream.CopyToAsync(memory);

            var fileInfo    = new FileInfo(pathToMedia);
            var newFileName = fileInfo.Name;

            var imagePath = Path.Combine(_personalFolder
                                       , newFileName); //, $"{_entryViewModel.Entry.Title}_{_entryViewModel.Entry.Id}.mp4");

            _mediaViewModel.Save(MediaType.Image
                               , memory.ToArray()
                               , imagePath );
            
            LeftToGetData = false;
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
    }

    private async void AddVideo()
    {
        if ( ! await ReadyToTakeVideo())
        {
            return;
        }
        
        try
        {
            var video       = await GetVideo();
            var pathToMedia = video.Path;
            if ( ! await WasFileReturnedFromCamera(video))
            {
                return;
            }
        
            using (var memory = new MemoryStream())
            {
                var stream = video.GetStream();
                await stream.CopyToAsync(memory);

                var fileInfo    = new FileInfo(pathToMedia);
                var newFileName = fileInfo.Name;

                var videoPath = Path.Combine(_personalFolder
                                           , newFileName); //, $"{_entryViewModel.Entry.Title}_{_entryViewModel.Entry.Id}.mp4");

                File.WriteAllBytes(videoPath
                                 , memory.ToArray());
                
                //Save Entry with Media to DB
                _mediaViewModel.Save(MediaType.Video
                                   , memory.ToArray()
                                   , videoPath);

            }
        
            LeftToGetData = false;
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
    }

    private async void MediaTypePicker_OnSelectedIndexChanged(object    sender
                                                            , EventArgs e)
    {
        var picker        = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex == -1)
            return;

        var selection = (string)picker.ItemsSource[selectedIndex];
        
        switch (selection)
        {
            case "Image":

                await AddMedia(MediaType.Image);
                
                break;

            case "Video":

                await AddMedia(MediaType.Video);
                
                break;
        }

        await PageNavigation.NavigateBackwards();
        MediaTypePicker.SelectedItem = null;
    }

    private async void VideoCell_OnTapped(object sender
                                        , EventArgs e)
    {
       
        // var label   = (Label)sender;
        // var mediaId = label.Text;

        // await PageNavigation.NavigateTo(nameof(MediaPage)
        //                               , nameof(MediaPage.EntryId)
        //                               , EntryId
        //                               , nameof(MediaPage.MediaId)
        //                               , mediaId);
    }

    private void ImageCell_OnTapped(object sender
                                         , EventArgs e)
    {
        
    }
}

