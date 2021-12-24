using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationExceptions;
using Syncfusion.DataSource.Extensions;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services
{
    public class DataAccess
    {
        private IDataStore Database { get; set; }

        public DataAccess(IDataStore database)
        {
            Database = database;
        }

        public IEnumerable<Journal> GetJournals()
        {
            return Database.GetJournals();
        }

        public Journal GetJournal(int journalId)
        {
            return App.Database.GetJournal(journalId);
        }

        public Entry GetEntry(int entryId)
        {
            return App.Database.GetEntry(entryId);
        }

        public void ResetUserData()
        {
            App.Database.DropUserTables();
            App.Database.CreateUserTables();
        }

        public void ResetAppData()
        {
            App.Database.DropAppTables();
            App.Database.CreateAppTables();
        }

        public void SaveJournalType(JournalType journalType)
        {
            App.Database.SaveJournalType(journalType);
        }

        public void SaveMood(Mood mood)
        {
            App.Database.SaveMood(mood);
        }

        public void AddJournalWithChildren(Journal journal)
        {
            App.Database.AddJournalWithChildren(journal);
        }

        public void SaveEntry(Entry entry
                            , int   journalId)
        {
            if (entry.Id == 0)
            {
                App.Database.AddEntryWIthChildren(entry, journalId);
            }
            else
            {
                App.Database.UpdateEntry(entry);
            }
        }

        public int DeleteJournal(ref Journal journalToDelete, bool deleteEntries = true)
        {
            foreach (var journalEntry in journalToDelete.Entries)
            {
                App.Database.DeleteEntry(journalEntry);
            }
            
            var numberDeleted = App.Database.DeleteJournal(ref journalToDelete);
            journalToDelete = null;

            return numberDeleted;
        }

        public void SaveJournal(Journal journal)
        {
            App.Database.SaveJournal(journal);
        }

        public List<JournalType> GetJournalTypesList()
        {
            return App.Database.GetJournalTypes()
                      .ToList();
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
            App.Database.UpdateJournal(journal);
        }

        public void AddJournalType(JournalType journalType)
        {
            App.Database.AddJournalType(journalType);
        }

        public IEnumerable<Entry> GetEntries()
        {
            return Database.GetEntries();
        }

        public Mood GetMood(string moodTitle)
        {
            return Database.GetMoods()
                           .FirstOrDefault(fields => fields.Title == moodTitle);
        }

        public Mood GetMood(int moodId)
        {
            return Database.GetMoods()
                           .FirstOrDefault(fields => fields.Id == moodId);
        }

        public void DeleteEntry(Entry entry)
        {
            Database.DeleteEntry(entry);
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

            var journalWord = "journal";
            if (numberOfJournalsWithJournalTypeRemoved == 1)
            {
                journalWord = "journals";
            }

            return $"{numberOfDeletedJournalTypes} {journalTypeWord} was deleted, and {numberOfJournalsWithJournalTypeRemoved} {journalWord} had that mood removed.";
        }

        public string DeleteJournalType(ref JournalType journalType)
        {
            var journalCount  = RemoveJournalTypeFromJournals(journalType);
            var numberDeleted = Database.DeleteJournalType(ref journalType);

            journalType = null;

            return GetDeletedJournalTypeMessage(numberDeleted
                                              , journalCount);
        }
    }
}
