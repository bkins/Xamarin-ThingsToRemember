using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Avails.D_Flat;
using Avails.Xamarin;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThingsToRemember.Views;

[QueryProperty(nameof(MediaId), nameof(MediaId))]
[QueryProperty(nameof(EntryId), nameof(EntryId))]
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MediaPage : ContentPage, IQueryAttributable
{
    public string EntryId { get ; set ; }
    public string MediaId { get ; set ; }

    private MediaViewModel _mediaViewModel;
    
    public MediaPage()
    {
        InitializeComponent();
    }
    
    public async void ApplyQueryAttributes(IDictionary<string, string> query)
    {
        MediaId = HttpUtility.UrlDecode(query[nameof(MediaId)]);
        EntryId = HttpUtility.UrlDecode(query[nameof(EntryId)]);

        if (MediaId.IsNullEmptyOrWhitespace()
         || EntryId.IsNullEmptyOrWhitespace())
        {
            await DisplayAlert("Could Not Load Page"
                             , "There was a problem retrieving data to load page."
                             , "OK");

        }

        _mediaViewModel = new MediaViewModel(int.Parse(EntryId)
                                           , int.Parse(MediaId));
        
        BindingContext = _mediaViewModel;

        MediaListView.ItemsSource = _mediaViewModel.MediaList;
    }

    private void VideoCell_OnTapped(object    sender
                                  , EventArgs e)
    {
        
    }
}