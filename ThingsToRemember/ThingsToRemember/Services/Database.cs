using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using ThingsToRemember.Models;
using ApplicationExceptions;
using Avails.Xamarin;
using SQLiteNetExtensions.Extensions;

namespace ThingsToRemember.Services
{

    //BENDO:  To sync DB from one device to another, look at:
    // *  https://stackoverflow.com/questions/62249498/xamarin-forms-backup-a-sqlite-database
    // *  https://social.msdn.microsoft.com/Forums/en-US/ec7e57f0-a4a3-4ad3-972e-dc3c84fa09d4/backup-a-sqlite-database?forum=xamarinforms

    public class Database : IDataStore
    {
        private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite
                                            | SQLiteOpenFlags.Create      // create the database if it doesn't exist
                                            | SQLiteOpenFlags.SharedCache // enable multi-threaded database access
                                            ;
        private readonly SQLiteConnection _database;
        public           string           Path { get; }

        public Database(string dbPath)
        {
            _database = new SQLiteConnection(dbPath, Flags);
            Path      = dbPath;

            CreateUserTables();
            CreateAppTables();
        }

        public int GetSizeFromPageCountByPageSize()
        {
            var size = _database.ExecuteScalar<int>("SELECT page_count * page_size as size FROM pragma_page_count(), pragma_page_size();");

            return size;
        }

        public int GetSizeFromFileInfo()
        {
            var size = (int) new FileInfo(Path).Length;

            return size;
        }

        public string GetFilePath()
        {
            return Path;
        }

        public void Close()
        {
            _database.Close();
        }

        public string GetFileName()
        {
            var fileName = new FileInfo(Path).Name;

            return fileName;
        }

        public void ResetAllData()
        {
            DropUserTables();
            CreateUserTables();
        }

        public void CreateUserTables()
        {
            _database.CreateTable<Journal>();
            _database.CreateTable<Entry>();
            _database.CreateTable<Media>();
        }

        public void CreateAppTables()
        {
            _database.CreateTable<Mood>();
            _database.CreateTable<JournalType>();
        }
        
        public void DropUserTables()
        {
            _database.DropTable<Journal>();
            _database.DropTable<Entry>();
            _database.DropTable<Media>();
        }

        public void DropAppTables()
        {
            _database.DropTable<Mood>();
            _database.DropTable<JournalType>();
        }

        public void SaveJournal(Journal journal)
        {
            if (journal.Id == 0)
            {
                AddJournal(journal);
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

        public void SaveMedia(Media media
                            , int   entryId)
        {
            if (media.Id == 0)
            {
                AddMediaWithChildren(media
                                   , entryId);
            }
            else
            {
                UpdateMedia(media);
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

        /// <summary>
        /// Adds a new Journal with any child (entries, journal type, etc) assigned to it.
        /// </summary>
        /// <param name="journal">The journal to insert</param>
        /// <exception cref="DuplicateRecordException">DuplicateRecordException</exception>
        public void AddJournalWithChildren(Journal journal)
        {
            //ValidateJournalType(journal.JournalType);

            _database.InsertWithChildren(journal);
        }

        public void AddEntry(Entry entry
                           , int   journalId)
        {
            entry.JournalId = journalId;
            _database.Insert(entry);
        }

        public void AddEntry(Entry entry)
        {
            _database.Insert(entry);
        }

        public void AddMedia(Media media)
        {
            _database.Insert(media);
        }
        
        public void AddEntryWIthChildren(Entry entry
                                       , int   journalId)
        {
            entry.JournalId         = journalId;
            entry.OriginalJournalId = journalId;
            
            _database.InsertWithChildren(entry);
        }
        
        private void AddMediaWithChildren(Media media
                                        , int   entryId)
        {
            media.EntryId = entryId;
            
            _database.InsertWithChildren(media);
        }

        public int AddJournal(Journal journal)
        {
            return _database.Insert(journal);
        }

        public Mood AddMood(Mood mood)
        {
            _database.Insert(mood);

            return _database.GetAllWithChildren<Mood>()
                            .FirstOrDefault(fields => fields.Title == mood.Title);

        }

        public int AddJournalType(JournalType journalType)
        {
            ValidateJournalType(journalType);

            return _database.Insert(journalType);
        }

        private void ValidateJournalType(JournalType journalType)
        {
            var types = GetJournalTypes();

            if (types.Any(fields => fields.Title == journalType.Title))
            {
                throw new DuplicateRecordException(nameof(JournalType)
                                                 , journalType.Title);
            }
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
            if (entry.OriginalJournalId == 0)
            {
                entry.OriginalJournalId = entry.JournalId;
            }
            
            _database.UpdateWithChildren(entry);
        }

        public void UpdateMedia(Media media)
        {
            _database.UpdateWithChildren(media);
        }

        #endregion

    #region Deletes

        /// <summary>
        /// Deletes the journal from the DB.
        /// If orphanChildren is FALSE, then the entries associated with the journal will be deleted as well.
        /// </summary>
        /// <param name="journal">The Journal to be deleted</param>
        /// <param name="orphanChildren">
        /// If True, Entries associated with the Journal will NOT be deleted.
        /// If False (default), Entries associated with the Journal WILL be deleted.
        /// </param>
        /// <returns>Number of records deleted</returns>
        public int DeleteJournal(ref Journal journal, bool orphanChildren = false)
        {
            foreach (var journalEntry in journal.Entries)
            {
                DeleteEntry(journalEntry);
            }

            var numberDeleted = _database.Delete(journal);
            journal = null;

            return numberDeleted;
        }
        
        public int DeleteEntry(Entry entry)
        {
            var numberDeleted = _database.Delete(entry);

            return numberDeleted;
        }
        
        public int DeleteEntry(ref Entry entry)
        {
            var numberDeleted = _database.Delete(entry);
            entry = null;

            return numberDeleted;
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

        /// <summary>
        /// Gets a journal based on the id passed in
        /// </summary>
        /// <param name="id">The id of the journal to return</param>
        /// <returns>The journal that has the id passed in</returns>
        /// <exception cref="SequenceContainsNoElementsException">SequenceContainsNoElementsException</exception>
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
        }

        public Journal GetJournal(string title)
        {
            try
            {
                var journal = _database.GetAllWithChildren<Journal>()
                                       .FirstOrDefault(fields => fields.Title == title);
                SetJournalsEntriesWithMoods(journal);
                
                return journal;
            }
            catch (InvalidOperationException invalidE)
            {
                throw new SequenceContainsNoElementsException($"Record could not be found. No record with an {nameof(title)} of {title}", nameof(Journal), invalidE);
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

            journal.Entries = entries.ToList(); //Overwrite entries.  For they will not have Moods
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

        public Entry GetEntry(string title, string text, DateTime createdOn)
        {
            try
            {
                return _database.GetAllWithChildren<Entry>()
                                .FirstOrDefault(fields => fields.Title == title
                                                       && fields.Text == text
                                                       && fields.CreateDateTime == createdOn);
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

        public IEnumerable<Media> GetMedia (int entryId)
        {
            try
            {
                return _database.GetAllWithChildren<Media>()
                                .Where(fields => fields.EntryId == entryId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        
        public IEnumerable GetEntriesWithMood(int moodId)
        {
            return _database.GetAllWithChildren<Entry>(fields => fields              != null && 
                                                                 fields.EntryMood    != null && 
                                                                 fields.EntryMood.Id == moodId);
        }
        
        public IEnumerable GetJournalsWithJournalTYpe(int journalTypeId)
        {
            return _database.GetAllWithChildren<Journal>(fields => fields.JournalTypeId == journalTypeId);
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

        public Mood GetMood(string title
                          , string emoji)
        {
            try
            {
                return _database.GetAllWithChildren<Mood>()
                                .FirstOrDefault(fields => fields.Title == title 
                                                       && fields.Emoji == emoji);
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
        public JournalType GetJournalType(string title)
        {
            try
            {
                return _database.GetAllWithChildren<JournalType>()
                                .FirstOrDefault(fields => fields.Title == title);
            }
            catch (InvalidOperationException invalidE)
            {
                throw new SequenceContainsNoElementsException($"Record could not be found with the {nameof(title)} of {title}"
                                                            , nameof(JournalType), invalidE);
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

        public IEnumerable<Media> GetAllMedia()
        {
            try
            {
                return _database.GetAllWithChildren<Media>();
            }
            catch (Exception e)
            {
                Logger.WriteLine(e.Message, Category.Error, e);

                throw;
            }
        }
    #endregion

    }
}
