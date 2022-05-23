using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationExceptions;
using Avails.D_Flat;
using Avails.Xamarin;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services;

public class BackupDatabase
{
    private readonly Database _destinationDatabase;
    private readonly Database _sourceDatabase;
    
    public BackupDatabase (Database destinationDatabase
                         , Database sourceDatabase)
    {
        _destinationDatabase = destinationDatabase;
        _sourceDatabase      = sourceDatabase;
        
        NukeAndPave();
    }

    public string GetDestinationDatabasePath()
    {
        return _destinationDatabase.Path;
    }

    public string GetSourceDatabasePath()
    {
        return _sourceDatabase.Path;
    }
    
    private void NukeAndPave()
    {
        _destinationDatabase.DropAppTables();
        _destinationDatabase.DropUserTables();
        
        _destinationDatabase.CreateUserTables();
        _destinationDatabase.CreateAppTables();
    }
    
    public void Backup()
    {
        NukeAndPave();
        
        BackupJournalTypes();
        BackupJournals();
        BackupMoods();
        BackupEntries();
        BackupMedia();
        BackupEntryMedia();
        
        JoinJournalsToEntries();
    }

    private void BackupMedia()
    {
        //Media table to Media table
        SaveMediaFromSourceToDestination();
    }

    private void BackupEntryMedia()
    {
        //Legacy media to new media table
        SaveImageFromEntryToMediaTable();
        SaveVideoFromEntryToMediaTable();
    }

    private void SaveMediaFromSourceToDestination()
    {
        var sourceMedia = _sourceDatabase.GetAllMedia();
        
        foreach (var media in sourceMedia)
        {
            var sourceEntry = _sourceDatabase.GetEntry(media.EntryId);

            var existingEntryId = GetDestinationEntry(sourceEntry.Title
                                                    , sourceEntry.Text
                                                    , sourceEntry.CreateDateTime).Id;
            var newMedia = new Media
                           {
                               Type          = media.Type
                             , MediaBytes    = media.MediaBytes
                             , MediaFileName = media.MediaFileName
                           };
            //Saves the new Media object and associates it will the entry id passed in.
            _destinationDatabase.SaveMedia(newMedia
                                         , existingEntryId);
        }
    }

    private void SaveVideoFromEntryToMediaTable()
    {
        var sourceVideosInEntries = _sourceDatabase.GetEntries()
                                                   .Where(fields => fields.HasVideo());

        foreach (var entryWithVideo in sourceVideosInEntries)
        {
            var newEntry = GetDestinationEntry(entryWithVideo.Title
                                             , entryWithVideo.Text
                                             , entryWithVideo.CreateDateTime);

            var newMedia = new Media
                           {
                               Type          = MediaType.Video
                             , MediaBytes    = entryWithVideo.Video
                             , MediaFileName = entryWithVideo.VideoFileName
                           };

            _destinationDatabase.SaveMedia(newMedia
                                         , newEntry.Id);
        }
    }

    private void SaveImageFromEntryToMediaTable()
    {
        var sourceImagesInEntries = _sourceDatabase.GetEntries()
                                                   .Where(fields => fields.HasImage());

        foreach (var entryWithImage in sourceImagesInEntries)
        {
            var existingEntry = GetDestinationEntry(entryWithImage.Title
                                                  , entryWithImage.Text
                                                  , entryWithImage.CreateDateTime);

            var newMedia = new Media
                           {
                               Type          = MediaType.Image
                             , MediaBytes    = entryWithImage.Image
                             , MediaFileName = entryWithImage.ImageFileName
                           };

            _destinationDatabase.SaveMedia(newMedia
                                         , existingEntry.Id);
        }
    }

    private void JoinJournalsToEntries()
    {
        try
        {
            var journals = GetDestinationJournals();
            foreach (var journal in journals)
            {
                journal.Entries = GetDestinationEntries().Where(fields => fields.JournalId == journal.Id) as List<Entry>;
            }
        }
        catch (Exception e)
        {
            throw new BackupValidationException("Error while joining Journals with Entries."
                                              , Logger.CompleteLog
                                              , e);
        }
    }

    private void BackupJournals()
    {
        var sourceJournals = _sourceDatabase.GetJournals();

        foreach (var journal in sourceJournals)
        {
            var newJournalType = GetDestinationJournalType(journal.JournalType.Title);

            var newJournal = new Journal
                             {
                                 Title         = journal.Title
                               , JournalTypeId = newJournalType.Id
                               , JournalType   = newJournalType
                               , Entries       = new List<Entry>()
                             };

            _destinationDatabase.AddJournal(newJournal);
        }
    }

    private void BackupJournalTypes()
    {
        foreach (var journalType in _sourceDatabase.GetJournalTypes())
        {
            var newJournalType = new JournalType { Title = journalType.Title };

            _destinationDatabase.AddJournalType(newJournalType);
        }
    }

    private void BackupEntries()
    {
        var sourceEntries = _sourceDatabase.GetEntries();

        foreach (var sourceEntry in sourceEntries)
        {
            if (sourceEntry.EntryMood is null)
            {
                continue;
            }
            
            var newMood = GetDestinationMood(sourceEntry.EntryMood.Title
                                , sourceEntry.EntryMood.Emoji);

            var sourceJournal = _sourceDatabase.GetJournal(sourceEntry.JournalId);
            var newJournal    = GetDestinationJournal(sourceJournal.Title);

            var newEntry = new Entry
                           {
                               Title          = sourceEntry.Title
                             , Text           = sourceEntry.Text
                             , CreateDateTime = sourceEntry.CreateDateTime
                             , MoodId         = newMood.Id
                             , EntryMood      = newMood
                             , Image          = sourceEntry.Image
                             , ImageFileName  = sourceEntry.ImageFileName
                             , Video          = sourceEntry.Video
                             , VideoFileName  = sourceEntry.VideoFileName
                             , JournalId      = newJournal.Id
                           };

            if (sourceEntry.OriginalJournalId != 0)
            {
                var sourceOriginalJournal = _sourceDatabase.GetJournal(sourceEntry.OriginalJournalId);
                var newOriginalJournal    = GetDestinationJournal(sourceOriginalJournal.Id);
                
                newEntry.OriginalJournalId = newOriginalJournal.Id;
                
            }
            
            _destinationDatabase.AddEntry(newEntry);
        }
    }

    private void BackupMoods()
    {
        foreach (var mood in _sourceDatabase.GetMoods())
        {
            var newMood = new Mood
                          {
                              Title = mood.Title
                            , Emoji = mood.Emoji
                          };

            _destinationDatabase.AddMood(newMood);
        }
    }
    
    [Obsolete($"Use this class ({nameof(BackupDatabase)}) to restore the backup by swapping the source and destination databases.")]
    public void RestoreDatabaseFromDestination()
    {
        _sourceDatabase.ResetAllData();
        
        foreach (var mood in GetDestinationMoods())
        {
            _sourceDatabase.AddMood(mood);
        }

        foreach (var journalType in GetDestinationJournalTypes())
        {
            _sourceDatabase.AddJournalType(journalType);
        }
        
        foreach (var journal in GetDestinationJournals())
        {
            _sourceDatabase.AddJournal(journal);
        }
        
        foreach (var entry in GetDestinationEntries())
        {
            _sourceDatabase.AddEntry(entry);
        }
    }

    private Journal GetDestinationJournal(int id)
    {
        try
        {
            return _destinationDatabase.GetJournal(id);
        }
        catch (InvalidOperationException invalidE)
        {
            throw new SequenceContainsNoElementsException("Record could not be found.", nameof(Journal), invalidE);
        }
        catch (Exception e)
        {
                
            Console.WriteLine(e);

            throw;
        }
    }

    private Journal GetDestinationJournal(string title)
    {
        try
        {
            return _destinationDatabase.GetJournal(title);
        }
        catch (InvalidOperationException invalidE)
        {
            throw new SequenceContainsNoElementsException("Record could not be found.", nameof(Journal), invalidE);
        }
        catch (Exception e)
        {  
            Console.WriteLine(e);

            throw;
        }
    }

    private IEnumerable<Journal> GetDestinationJournals(bool forceRefresh = false)
    {
        return _destinationDatabase.GetJournals();
    }

    private Mood GetDestinationMood(string title, string emoji)
    {
        try
        {
            return _destinationDatabase.GetMood(title, emoji);
        }
        catch (InvalidOperationException invalidE)
        {
            throw new SequenceContainsNoElementsException("Record could not be found.", nameof(Mood), invalidE);
        }
        catch (Exception e)
        {
                
            Console.WriteLine(e);

            throw;
        }
    }

    private IEnumerable<Mood> GetDestinationMoods(bool forceRefresh = false)
    {
        return _destinationDatabase.GetMoods();
    }

    private JournalType GetDestinationJournalType(string title)
    {
        try
        {
            return _destinationDatabase.GetJournalType(title);
        }
        catch (InvalidOperationException invalidE)
        {
            throw new SequenceContainsNoElementsException("Record could not be found.", nameof(JournalType), invalidE);
        }
        catch (Exception e)
        {
                
            Console.WriteLine(e);

            throw;
        }
    }

    private IEnumerable<JournalType> GetDestinationJournalTypes(bool forceRefresh = false)
    {
        try
        {
            return _destinationDatabase.GetJournalTypes();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }

    private Entry GetDestinationEntry(string   title
                         , string   text
                         , DateTime createDateTime)
    {
        var entry = _destinationDatabase.GetEntry(title, text, createDateTime);

        return entry ?? new Entry();
    }

    private IEnumerable<Entry> GetDestinationEntries(bool forceRefresh = false)
    {
        return _destinationDatabase.GetEntries();
    }

    public void ValidateBackup()
    {
        var writeToConsoleSetting = Logger.WriteToConsole;

        Logger.WriteToConsole = true;

        try
        {
            ValidateMoods();
            ValidateJournalTypes();
            ValidateJournals();
            ValidateEntries();
            ValidateMedia();
            ValidateEntryMedia();
        }
        catch (BackupValidationException e)
        {
            Logger.WriteLine(e.ToString()
                           , Category.Error
                           , e);
        }
        
        Logger.WriteLine("Validation completed successfully.", Category.Information);
        
        Logger.WriteToConsole = writeToConsoleSetting;
    }

    private void ValidateMedia()
    {
        const string tableNameForValidating = "Media";
        
        Logger.WriteLine($"Validating {tableNameForValidating}..."
                       , Category.Information);

        var backedUp    = _destinationDatabase.GetAllMedia().Count();
        var source = _sourceDatabase.GetAllMedia().Count();

        var message       = $"{tableNameForValidating} counts match: {backedUp == source}.";
        
        Logger.WriteLine(message, Category.Information);
        
        if (backedUp != source)
        {
            throw new BackupValidationException(message
                                              , Logger.CompleteLog);
        }
    }

    private void ValidateEntryMedia()
    {
        const string tableNameForValidating = "Entry Media";
        
        Logger.WriteLine($"Validating {tableNameForValidating}..."
                       , Category.Information);

        var source = _sourceDatabase.GetEntries()
                                    .Count(fields => fields.HasMedia());

        var backedUp = _destinationDatabase.GetEntries()
                                           .Count(fields => fields.HasMedia());
        
        var message = $"{tableNameForValidating} counts match: {backedUp == source}.";
        
        Logger.WriteLine(message, Category.Information);

        if (backedUp != source)
        {
            throw new BackupValidationException(message
                                              , Logger.CompleteLog);
        }
    }

    private void ValidateMoods()
    {
        const string tableNameForValidating = "Moods";
        
        Logger.WriteLine($"Validating {tableNameForValidating}..."
                       , Category.Information);

        var backedUp    = _destinationDatabase.GetMoods();
        var sourceTable = _sourceDatabase.GetMoods();

        var listOfSource = sourceTable.ToList();

        var backedUpMoods = backedUp.ToList();

        Logger.WriteLine($"{tableNameForValidating} counts match: {backedUpMoods.Count == listOfSource.Count()}"
                       , Category.Information);

        foreach (var source in listOfSource)
        {
            var found = backedUpMoods.Any(fields => fields.Title == source.Title && fields.Emoji == source.Emoji);

            Logger.WriteLine($"{tableNameForValidating} '{source.Title}' found in source: {found}."
                           , Category.Information);

            if (! found)
            {
                var message = $"{tableNameForValidating} not valid!!";

                Logger.WriteLine(message
                               , Category.Error);

                throw  new BackupValidationException(message, Logger.CompleteLog);
            }
        }
    }
    
    private void ValidateJournalTypes()
    {
        const string tableNameForValidating = "JournalTypes";
        
        Logger.WriteLine($"Validating {tableNameForValidating}..."
                       , Category.Information);

        var backedUp    = _destinationDatabase.GetJournalTypes();
        var sourceTable = _sourceDatabase.GetJournalTypes();

        var listOfSource = sourceTable.ToList();

        var backedUpJournalTypes = backedUp.ToList();

        Logger.WriteLine($"{tableNameForValidating} counts match: {backedUpJournalTypes.Count == listOfSource.Count()}"
                       , Category.Information);

        foreach (var source in listOfSource)
        {
            var found = backedUpJournalTypes.Any(fields => fields.Title == source.Title);

            Logger.WriteLine($"{tableNameForValidating} '{source.Title}' found in source: {found}."
                           , Category.Information);

            if (! found)
            {
                var message = $"{tableNameForValidating} not valid!!";

                Logger.WriteLine(message
                               , Category.Error);

                throw  new BackupValidationException(message, Logger.CompleteLog);
            }
        }
    }
    
    private void ValidateJournals()
    {
        const string tableNameForValidating = "Journals";
        
        Logger.WriteLine($"Validating {tableNameForValidating}..."
                       , Category.Information);

        var backedUp    = _destinationDatabase.GetJournals();
        var sourceTable = _sourceDatabase.GetJournals();

        var listOfSource     = sourceTable.ToList();
        var backedUpJournals = backedUp.ToList();

        Logger.WriteLine($"{tableNameForValidating} counts match: {backedUpJournals.Count == listOfSource.Count()}"
                       , Category.Information);

        foreach (var source in listOfSource)
        {
            var found = backedUpJournals.Any(fields => fields.Title == source.Title
                                                    && fields.JournalTypeId == source.JournalTypeId);

            Logger.WriteLine($"{tableNameForValidating} '{source.Title}' found in source: {found}."
                           , Category.Information);

            if (! found)
            {
                var message = $"{tableNameForValidating} not valid!!";

                Logger.WriteLine(message
                               , Category.Error);

                throw  new BackupValidationException(message, Logger.CompleteLog);
            }
        }
    }
    
    private void ValidateEntries()
    {
        const string tableNameForValidating = "Entries";
        
        Logger.WriteLine($"Validating {tableNameForValidating}..."
                       , Category.Information);

        var backedUp    = _destinationDatabase.GetEntries();
        var sourceTable = _sourceDatabase.GetEntries();

        var listOfSource = sourceTable.ToList();

        //var differences = backedUp.Except(listOfSource, new EqualityComparer());
        var correctedSource = listOfSource.Where(fields => fields.JournalId != 0);

        var listOfCorrectedSource = correctedSource.ToList();

        var backedUpEntries = backedUp.ToList();
        var message    = $"{tableNameForValidating} counts match: {backedUpEntries.Count == listOfCorrectedSource.Count()}";
        Logger.WriteLine(message
                       , Category.Information);
        
        if (backedUpEntries.Count() != listOfCorrectedSource.Count())
        {
            throw new BackupValidationException(message
                                              , Logger.CompleteLog);
        }
    }

    public bool Equals(Entry left
                     , Entry right)
    {
        return left.Title == right.Title
            && left.Text == right.Text
            && left.EntryMood != null
            && left.CreateDateTime == right.CreateDateTime
            && left.Image.SafeSequenceEqual(right.Image)
            && left.ImageFileName == right.ImageFileName
            && left.Video.SafeSequenceEqual(right.Video)
            && left.VideoFileName == right.VideoFileName;
    }
}

public class EqualityComparer : IEqualityComparer<Entry>
{
    public bool Equals(Entry x
                     , Entry y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (x.GetType() != y.GetType())
            return false;

        return x.Title == y.Title
            && x.Text == y.Text
            && x.CreateDateTime.Equals(y.CreateDateTime)
            && Equals(x.Image, y.Image)
            && x.ImageFileName == y.ImageFileName
            && Equals(x.Video, y.Video)
            && x.VideoFileName == y.VideoFileName;
    }

    public int GetHashCode(Entry obj)
    {
        unchecked
        {
            var hashCode = ( obj.Title != null ? obj.Title.GetHashCode() : 0 );
            hashCode = ( hashCode * 397 ) ^ ( obj.Text != null ? obj.Text.GetHashCode() : 0 );
            hashCode = ( hashCode * 397 ) ^ obj.CreateDateTime.GetHashCode();
            hashCode = ( hashCode * 397 ) ^ ( obj.Image != null ? obj.Image.GetHashCode() : 0 );
            hashCode = ( hashCode * 397 ) ^ ( obj.ImageFileName != null ? obj.ImageFileName.GetHashCode() : 0 );
            hashCode = ( hashCode * 397 ) ^ ( obj.Video != null ? obj.Video.GetHashCode() : 0 );
            hashCode = ( hashCode * 397 ) ^ ( obj.VideoFileName != null ? obj.VideoFileName.GetHashCode() : 0 );

            return hashCode;
        }
    }
}
