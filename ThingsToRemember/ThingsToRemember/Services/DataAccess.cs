using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApplicationExceptions;
using Syncfusion.DataSource.Extensions;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services
{
    public class DataAccess
    {
        public  string     DatabaseLocation => GetDatabaseFolderPath();
        private IDataStore Database         { get; set; }
        public  string     DatabaseFileName => GetDatabaseFileName();

        public DataAccess(IDataStore database)
        {
            Database = database;
        }

        public string GetDatabaseLocation()
        {
            return Database.GetFilePath();
        }

        public string GetDatabaseFolderPath()
        {
            var folderPath = Database.GetFilePath();
            folderPath = Path.GetDirectoryName(folderPath);

            return folderPath;
        }
        public string GetDatabaseFileName()
        {
            return Database.GetFileName();
        }

        public int GetDatabaseSize()
        {
            return Database.GetSizeFromPageCountByPageSize();
        }

        public int GetSizeFromFileInfo()
        {
            return Database.GetSizeFromFileInfo();
        }

        public IEnumerable<Journal> GetJournals()
        {
            var journals = Database.GetJournals();

            return journals;
        }

        public Journal GetJournal(int journalId)
        {
            if (journalId == 0)
            {
                return new Journal();
            }
            return Database.GetJournal(journalId);
        }

        public Entry GetEntry(int entryId)
        {
            return Database.GetEntry(entryId);
        }

        public void ResetUserData()
        {
            Database.DropUserTables();
            Database.CreateUserTables();
        }

        public void ResetAppData()
        {
            Database.DropAppTables();
            Database.CreateAppTables();
        }

        public void SaveJournalType(JournalType journalType)
        {
            Database.SaveJournalType(journalType);
        }

        public void SaveMood(Mood mood)
        {
            Database.SaveMood(mood);
        }

        public void AddJournalWithChildren(Journal journal)
        {
            Database.AddJournalWithChildren(journal);
        }

        public void AddJournal(Journal journal)
        {
            Database.AddJournal(journal);
        }

        public void AddMedia(Media media, int entryId)
        {
            Database.SaveMedia(media, entryId);
        }
        
        public void SaveEntry(Entry entry
                            , int   journalId)
        {
            if (entry.Id == 0)
            {
                Database.AddEntry(entry, journalId);
            }
            else
            {
                Database.UpdateEntry(entry);
            }
        }

        public int DeleteJournal(ref Journal journalToDelete, bool deleteEntries = true)
        {
            foreach (var journalEntry in journalToDelete.Entries)
            {
                Database.DeleteEntry(journalEntry);
            }
            
            var numberDeleted = Database.DeleteJournal(ref journalToDelete);
            journalToDelete = null;

            return numberDeleted;
        }

        public void SaveJournal(Journal journal)
        {
            Database.SaveJournal(journal);
        }

        public List<JournalType> GetJournalTypesList()
        {
            var journalTypes = Database.GetJournalTypes()
                                       .ToList();
            return journalTypes;
        }

        public List<string> GetJournalTypeNameList()
        {
            var typeList = new List<string>();
            var allTypesList = GetJournalTypesList();
            
            foreach (var type in allTypesList.Where(type => ! typeList.Contains(type.Title)))
            {
                typeList.Add(type.Title);
            }
            return typeList;
        }

        public void UpdateJournal(Journal journal)
        {
            Database.UpdateJournal(journal);
        }

        public void AddJournalType(JournalType journalType)
        {
            Database.AddJournalType(journalType);
        }

        public void UpdateMedia(Media media)
        {
            Database.UpdateMedia(media);
        }

        public void SaveMedia(Media media, int entryId)
        {
            Database.SaveMedia(media, entryId);
        }
        
        public IEnumerable<Entry> GetEntries()
        {
            return Database.GetEntries();
        }

        public IEnumerable<Media> GetMediaByEntry(int entryId)
        {
            return Database.GetMedia(entryId);
        }
        
        public Mood GetMood(string moodTitle)
        {
            return Database.GetMoods()
                           .FirstOrDefault(fields => fields.Title == moodTitle);
        }

        public Mood GetMood(string moodTitle
                          , string moodEmoji)
        {
            return Database.GetMoods()
                           .FirstOrDefault(fields => fields.Title == moodTitle 
                                                  && fields.Emoji == moodEmoji);
        }

        public Mood GetMood(int moodId)
        {
            return Database.GetMoods()
                           .FirstOrDefault(fields => fields.Id == moodId);
        }

        public int DeleteEntry(ref Entry entryToDelete)
        {
            var numberDeleted = Database.DeleteEntry(ref entryToDelete);
            entryToDelete = null;

            return numberDeleted;
        }

        public JournalType GetJournalType(string typeTitle)
        {
            return Database.GetJournalTypes().FirstOrDefault(fields=>fields.Title == typeTitle);
        }

        public JournalType GetJournalType(int typeId)
        {
            return Database.GetJournalTypes()
                           .FirstOrDefault(fields => fields.Id == typeId);
        }

        public List<Mood> GetMoods()
        {
            return Database.GetMoods()
                           .OrderBy(fields=>fields.Title)
                           .ToList();
        }

        public List<Media> GetAllMedia()
        {
            return Database.GetAllMedia()
                           .ToList();
        }
        
        public Mood AddMood(string moodTitle
                          , string moodEmoji)
        {
            if (Database.GetMoods().Any(fields => fields.Title == moodTitle))
            {
                throw new DuplicateRecordException(nameof(Mood)
                                                 , moodTitle);
            }

            var mood = new Mood
                       {
                           Title = moodTitle
                         , Emoji = moodEmoji
                       };

            return Database.AddMood(mood);
        }

        public string DeleteMood(ref Mood moodToDelete)
        {
            var entryCount = RemoveMoodFromEntries(moodToDelete);

            var numberDeleted = Database.DeleteMood(ref moodToDelete);

            moodToDelete = null;

            return GetDeletedMoodMessage(numberDeleted
                                       , entryCount);
        }

        private int RemoveMoodFromEntries(Mood moodToDelete)
        {
            var                moodId = moodToDelete.Id;
            IEnumerable<Entry> entriesWithMood;

            try
            {
                entriesWithMood = Database.GetEntriesWithMood(moodId).ToList<Entry>();
            }
            catch (Exception )
            {
                return 0;
            }
            
            var entryCount = 0;

            foreach (var entry in entriesWithMood)
            {
                entry.EntryMood = new Mood();
                entry.MoodId    = 0;

                entryCount++;
            }

            return entryCount;
        }
        
        private int RemoveJournalTypeFromJournals(JournalType journalTypeToDelete)
        {
            var                  journalTypeId = journalTypeToDelete.Id;
            IEnumerable<Journal> journalsWithJournalType;

            try
            {
                journalsWithJournalType = Database.GetJournalsWithJournalTYpe(journalTypeId).ToList<Journal>();
            }
            catch (Exception )
            {
                return 0;
            }
            
            var journalCount = 0;

            foreach (var journal in journalsWithJournalType)
            {
                journal.JournalTypeId = 0;
                journal.JournalType   = new JournalType();
                journalCount++;
            }

            return journalCount;
        }

        private string GetDeletedMoodMessage(int numberOfDeletedMoods
                                           , int numberOfEntriesWithMoodRemoved)
        {
            var moodWord = "moods";
            if (numberOfDeletedMoods == 1)
            {
                moodWord = "mood";
            }

            var entryWord = "entries";
            if (numberOfEntriesWithMoodRemoved == 1)
            {
                entryWord = "entry";
            }

            return $"{numberOfDeletedMoods} {moodWord} was deleted, and {numberOfEntriesWithMoodRemoved} {entryWord} had that mood removed.";
        }
        
        private string GetDeletedJournalTypeMessage(int numberOfDeletedJournalTypes
                                                  , int numberOfJournalsWithJournalTypeRemoved)
        {
            var journalTypeWord = "journal types";
            if (numberOfDeletedJournalTypes == 1)
            {
                journalTypeWord = "journal type";
            }

            var journalWord = "journals";
            if (numberOfJournalsWithJournalTypeRemoved == 1)
            {
                journalWord = "journal";
            }

            return $"{numberOfDeletedJournalTypes} {journalTypeWord} was deleted, and {numberOfJournalsWithJournalTypeRemoved} {journalWord} had that journal type removed.";
        }

        public string DeleteJournalType(ref JournalType journalType)
        {
            var journalCount  = RemoveJournalTypeFromJournals(journalType);
            var numberDeleted = Database.DeleteJournalType(ref journalType);

            journalType = null;

            return GetDeletedJournalTypeMessage(numberDeleted
                                              , journalCount);
        }

        public void CloseDatabaseConnection()
        {
            Database.Close();
        }

    }
}
