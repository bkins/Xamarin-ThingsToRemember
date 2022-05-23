using System.Collections.Generic;
using System.Linq;
using ApplicationExceptions;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels;

public class MediaViewModel : BaseViewModel
{
    public  IEnumerable<Media> MediaList  { get; set; }
    private int                EntryId    { get; set; }
    
    public MediaViewModel (int entryId)
    {
        EntryId   = entryId;
        
        MediaList = DataAccessLayer.GetMediaByEntry(EntryId);

        SetMediaInList();
    }

    public MediaViewModel (int entryId, int mediaId)
    {
        EntryId = entryId;
        MediaList = DataAccessLayer.GetMediaByEntry(entryId)
                                   .Where(fields => fields.Id == mediaId);
        SetMediaInList();
    }

    private void SetMediaInList()
    {
        foreach (var media in MediaList)
        {
            media.SetMedia();
        }
    }
    public void Save(MediaType mediaType
                   , byte[]    mediaBytes
                   , string    mediaFileName)
    {
        ValidateFileName(mediaFileName);
        
        var newMedia = new Media
                       {
                           Type          = mediaType
                         , MediaBytes    = mediaBytes
                         , MediaFileName = mediaFileName
                       };

        DataAccessLayer.AddMedia(newMedia
                               , EntryId);
        //media Id is not known until the Media is added to the DB
        //Once added we can put the Id in the file name
        newMedia.MediaFileName = newMedia.MediaFileName.Replace("~id~", newMedia.Id.ToString());
        
        DataAccessLayer.SaveMedia(newMedia, EntryId);
    }

    private static void ValidateFileName(string mediaFileName)
    {
        if ( ! mediaFileName.Contains("~id~"))
        {
            throw new BadMediaFileNameException(mediaFileName
                                              , "~id~");
        }
    }
}