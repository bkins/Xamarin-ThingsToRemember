﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using ThingsToRemember.Models;
using ApplicationExceptions;
using SQLiteNetExtensions.Extensions;

namespace ThingsToRemember.Services
{

    //BENDO:  To sync DB from one device to another, look at:
    // *  https://stackoverflow.com/questions/62249498/xamarin-forms-backup-a-sqlite-database
    // *  https://social.msdn.microsoft.com/Forums/en-US/ec7e57f0-a4a3-4ad3-972e-dc3c84fa09d4/backup-a-sqlite-database?forum=xamarinforms

    public class Database : IDataStore
    {
        private readonly SQLiteConnection _database;
        
        public Database(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);

            CreateTables();
        }

        public void CreateTables()
        {
            _database.CreateTable<Journal>();
            _database.CreateTable<Entry>();
            _database.CreateTable<JournalType>();
            _database.CreateTable<Mood>();
        }
        
        public void DropTables()
        {
            _database.DropTable<Journal>();
            _database.DropTable<Entry>();
            _database.DropTable<JournalType>();
            _database.DropTable<Mood>();
        }

        public void SaveJournal(Journal journal)
        {
            if (journal.Id == 0)
            {
                AddJournalWithChildren(journal);
            }
            else
            {
                UpdateJournal(journal);
            }
        }

        public void SaveJournalType(JournalType journalType)
        {
            if (journalType.Id == 0)
            {
                AddJournalType(journalType);
            }
            else
            {
                UpdateJournalType(journalType);
            }
        }

        public void SaveEntry(Entry entry, int journalId)
        {
            if (entry.Id == 0)
            {
                AddEntryWIthChildren(entry, journalId);
            }
            else
            {
                UpdateEntry(entry);
            }
        }

        public void SaveMood(Mood mood)
        {
            if (mood.Id == 0)
            {
                AddMood(mood);
            }
            else
            {
                UpdateMood(mood);
            }
        }
    #region Adds

        public void AddJournalWithChildren(Journal journal)
        {
            _database.InsertWithChildren(journal);
        }

        public void AddEntryWIthChildren(Entry entry
                                       , int   journalId)
        {
            entry.JournalId = journalId;
            _database.InsertWithChildren(entry);
        }
        public int AddJournal(Journal journal)
        {
            return _database.Insert(journal);
            
        }

        public int AddMood(Mood mood)
        {
            return _database.Insert(mood);
            
        }

        public int AddJournalType(JournalType journalType)
        {
            return _database.Insert(journalType);
            
        }
        
    #endregion

    #region Updates

        public void UpdateJournal(Journal journal)
        {
            _database.UpdateWithChildren(journal);
            _database.Commit();   
        }

        public int UpdateMood(Mood mood)
        {
            return _database.Update(mood);
            
        }

        public int UpdateJournalType(JournalType journalType)
        {
            return _database.Update(journalType);
            
        }

        public void UpdateEntry(Entry entry)
        {
            _database.UpdateWithChildren(entry);
            
        }

        #endregion

    #region Deletes

        public int DeleteJournal(ref Journal journal)
        {
            foreach (var journalEntry in journal.Entries)
            {
                DeleteEntry(journalEntry);
            }

            var journalId = _database.Delete(journal);
            journal = null;

            return journalId;
        }

        public int DeleteEntry(Entry entry)
        {
            var entryId = _database.Delete(entry);

            return entryId;
        }

        public int DeleteEntry(ref Entry entry)
        {
            var entryId = _database.Delete(entry);
            entry = null;

            return entryId;
        }

        public int DeleteMood(ref Mood mood)
        {
            var moodId = _database.Delete(mood);
            mood = null;

            return moodId;
            
        }

        public int DeleteJournalType(ref JournalType journalType)
        {
            var journalTypeId = _database.Delete(journalType);
            journalType = null;

            return journalTypeId;

        }
        
    #endregion

    #region Selects

        public Journal GetJournal(int id)
        {
            try
            {
                var journal = _database.GetWithChildren<Journal>(id);
                SetJournalsEntriesWithMoods(journal);

                return journal;
            }
            catch (InvalidOperationException invalidE)
            {
                throw new SequenceContainsNoElementsException($"Record could not be found. No record with an {nameof(id)} of {id}", nameof(Journal), invalidE);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public IEnumerable<Journal>  GetJournals(bool forceRefresh = false)
        {
            try
            {
                //Problem:  When Getting with children, the returned Journal has its child Entry, but the Entry does not have its Mood
                //return _database.GetAllWithChildren<Journal>();
                //Workaround:  Get Journal (with children), Get each Entry (with children) that have that JournalId.

                var journals = _database.GetAllWithChildren<Journal>();

                foreach (var journal in journals)
                {
                    SetJournalsEntriesWithMoods(journal);
                }

                return journals;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        private void SetJournalsEntriesWithMoods(Journal journal)
        {
            var entries = GetEntries()
                         .ToList()
                         .Where(item => item.JournalId == journal.Id);

            journal.Entries = entries.ToList(); //Overwrite entries.  For they will not Moods
        }

        public Entry GetEntry(int id)
        {
            try
            {
                return _database.GetWithChildren<Entry>(id);
            }
            catch (InvalidOperationException invalidE)
            {
                throw new SequenceContainsNoElementsException("Record could not be found.", nameof(Entry), invalidE);
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e);

                throw;
            }
        }

        public IEnumerable<Entry> GetEntries(bool forceRefresh = false)
        {
            try
            {
                return _database.GetAllWithChildren<Entry>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        
        public Mood GetMood(int id)
        {
            try
            {
                return _database.GetWithChildren<Mood>(id);
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

        public IEnumerable<Mood> GetMoods(bool forceRefresh = false)
        {
            try
            {
                return _database.GetAllWithChildren<Mood>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public JournalType GetJournalType(int id)
        {
            try
            {
                return _database.GetWithChildren<JournalType>(id);
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

        public IEnumerable<JournalType> GetJournalTypes(bool forceRefresh = false)
        {
            try
            {
                return _database.GetAllWithChildren<JournalType>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

    #endregion
        
    }
}
