using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;

namespace ThingsToRemember.Models;

[Table(nameof(Media))]
public class Media
{
    [PrimaryKey, AutoIncrement]
    public int       Id            { get; set; }
    public byte[]    MediaBytes    { get; set; }
    public string    MediaFileName { get; set; }
    public MediaType Type          { get; set; }
    
    [ForeignKey(typeof(Entry))]
    public int EntryId { get; set; }
    
    [Ignore]
    public ImageSource Image { get; set; }
    [Ignore]
    public MediaSource Video { get; set; }

    public void SetMedia()
    {
        if (Type == MediaType.Image)
        {
            Image = GetImage();
        }
        else
        {
            Video = GetVideo();
        }
    }
    //ImageSource.FromStream(() => new MemoryStream(_entryViewModel.Entry.Image));
    public ImageSource GetImage()
    {
        return ImageSource.FromStream(()=> new MemoryStream(MediaBytes));
    }
    
    //VideoFromCamera.Source = MediaSource.FromFile(_entryViewModel.Entry.VideoFileName);
    public MediaSource GetVideo()
    {
        var memory = new MemoryStream();

        if (MediaBytes == null)
        {
            memory = new MemoryStream();
        }
        else
        {
            memory = new MemoryStream(MediaBytes);
        }
        
        using var    outputStream = File.Create(MediaFileName);
        
        memory.CopyTo(outputStream);
        memory.Dispose();
        
        return MediaSource.FromFile(MediaFileName);
    }
}

public enum MediaType
{
      Image
    , Video 
}