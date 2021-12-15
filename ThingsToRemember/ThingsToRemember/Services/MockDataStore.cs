using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services
{
    public class MockDataStore : IDataStore
    {
        private readonly List<Journal>     _journals;
        private readonly List<JournalType> _types;
        private readonly List<Entry>       _entries;
        private readonly List<Mood>        _moods;

        public MockDataStore()
        {
            //Populate database
            _moods = new List<Mood>()
                     {
                         new Mood
                         {
                             Id    = 1
                           , Title = "Happy"
                           , Emoji = ":)"
                         }
                       , new Mood
                         {
                             Id    = 2
                           , Title = "Sad"
                           , Emoji = ":("
                         }
                       , new Mood
                         {
                             Id    = 3
                           , Title = "Mad"
                           , Emoji = ">;("
                         }
                       , new Mood
                         {
                             Id    = 4
                           , Title = "Amazed"
                           , Emoji = ":O"
                         }
                     };

            _entries = new List<Entry>()
                       {
                           new Entry
                           {

                               Id             = 1
                             , Title          = "Entry 1"
                             , Text           = "Entry 1 text"
                             , CreateDateTime = DateTime.Now
                             , EntryMood      = _moods.First(mood => mood.Id == 1)

                           }
                         , new Entry
                           {
                               Id             = 2
                             , Title          = "Entry 2"
                             , Text           = "Entry 2 text"
                             , CreateDateTime = DateTime.Now.AddDays(1)
                             , EntryMood      = _moods.First(mood => mood.Id == 2)

                           }
                         , new Entry
                           {

                               Id             = 3
                             , Title          = "Entry 3"
                             , Text           = "Entry 3 text"
                             , CreateDateTime = DateTime.Now.AddDays(2)
                             , EntryMood      = _moods.First(mood => mood.Id == 3)

                           }
                         , new Entry
                           {
                               Id             = 4
                             , Title          = "Entry 4"
                             , Text           = "Entry 4 text"
                             , CreateDateTime = DateTime.Now.AddDays(3)
                             , EntryMood      = _moods.First(mood => mood.Id == 4)

                           }
                         , new Entry
                           {

                               Id             = 5
                             , Title          = "Entry 5"
                             , Text           = "Entry 5 text"
                             , CreateDateTime = DateTime.Now.AddMonths(1)
                             , EntryMood      = _moods.First(mood => mood.Id == 1)

                           }
                         , new Entry
                           {
                               Id             = 6
                             , Title          = "Entry 6"
                             , Text           = "Entry 6 text"
                             , CreateDateTime = DateTime.Now.AddMonths(2)
                             , EntryMood      = _moods.First(mood => mood.Id == 2)

                           }
                         , new Entry
                           {

                               Id             = 7
                             , Title          = "Entry 7"
                             , Text           = "Entry 7 text"
                             , CreateDateTime = DateTime.Now.AddMonths(3)
                             , EntryMood      = _moods.First(mood => mood.Id == 3)

                           }
                         , new Entry
                           {
                               Id             = 8
                             , Title          = "Entry 8"
                             , Text           = "Entry 8 text"
                             , CreateDateTime = DateTime.Now.AddMonths(4)
                             , EntryMood      = _moods.First(mood => mood.Id == 4)

                           }
                         , new Entry
                           {

                               Id             = 9
                             , Title          = "Entry 9"
                             , Text           = "Entry 9 text"
                             , CreateDateTime = DateTime.Now.AddYears(1)
                             , EntryMood      = _moods.First(mood => mood.Id == 1)

                           }
                         , new Entry
                           {
                               Id             = 10
                             , Title          = "Entry 10"
                             , Text           = "Entry 10 text"
                             , CreateDateTime = DateTime.Now.AddYears(2)
                             , EntryMood      = _moods.First(mood => mood.Id == 2)

                           }
                         , new Entry
                           {

                               Id             = 11
                             , Title          = "Entry 11"
                             , Text           = "Entry 11 text"
                             , CreateDateTime = DateTime.Now.AddYears(3)
                             , EntryMood      = _moods.First(mood => mood.Id == 3)

                           }
                         , new Entry
                           {
                               Id             = 12
                             , Title          = "Entry 12"
                             , Text           = "Entry 12 text"
                             , CreateDateTime = DateTime.Now.AddYears(4)
                             , EntryMood      = _moods.First(mood => mood.Id == 4)

                           }
                       };

            _types = new List<JournalType>()
                     {
                         new JournalType {Id = 1, Title = "Type 1"},
                         new JournalType {Id = 2, Title = "Type 2"},
                         new JournalType {Id = 3, Title = "Type 3"},
                         new JournalType {Id = 4, Title = "Type 4"},
                         new JournalType {Id = 5, Title = "Type 5"},
                         new JournalType {Id = 6, Title = "Type 6"}
                     };

            _journals = new List<Journal>()
                        {
                            new Journal
                            {
                                Id    = 1
                              , Title = "First Journal"
                              , JournalType  = _types.First(type => type.Id == 1)
                              , Entries = new List<Entry>()
                                          {
                                              _entries.First(entry => entry.Id == 1)
                                            , _entries.First(entry => entry.Id == 2)
                                          }
                            }
                          , new Journal
                            {
                                Id    = 2
                              , Title = "Second Journal"
                              , JournalType  = _types.First(type => type.Id == 2)
                              , Entries = new List<Entry>()
                                          {
                                              _entries.First(entry => entry.Id == 3)
                                            , _entries.First(entry => entry.Id == 4)
                                          }
                            }
                          , new Journal
                            {
                                Id    = 3
                              , Title = "Third Journal"
                              , JournalType  = _types.First(type => type.Id == 3)
                              , Entries = new List<Entry>()
                                          {
                                              _entries.First(entry => entry.Id == 5)
                                            , _entries.First(entry => entry.Id == 6)
                                          }
                            }
                          , new Journal
                            {
                                Id    = 4
                              , Title = "Fourth Journal"
                              , JournalType  = _types.First(type => type.Id == 4)
                              , Entries = new List<Entry>()
                                          {
                                              _entries.First(entry => entry.Id == 7)
                                            , _entries.First(entry => entry.Id == 8)
                                          }
                            }
                          , new Journal
                            {
                                Id    = 5
                              , Title = "Fifth Journal"
                              , JournalType  = _types.First(type => type.Id == 5)
                              , Entries = new List<Entry>()
                                          {
                                              _entries.First(entry => entry.Id == 9)
                                            , _entries.First(entry => entry.Id == 10)
                                          }
                            }
                          , new Journal
                            {
                                Id    = 6
                              , Title = "Sixth Journal"
                              , JournalType  = _types.First(type => type.Id == 6)
                              , Entries = new List<Entry>()
                                          {
                                              _entries.First(entry => entry.Id == 11)
                                            , _entries.First(entry => entry.Id == 12)
                                          }
                            }
                        };
        }

        public int AddJournal(Journal journal)
        {
            _journals.Add(journal);

            return journal.Id;
        }

        public void AddEntryWIthChildren(Entry entry
                                       , int   journalId)
        {
            entry.JournalId = journalId; //BENDO: is this needed?

            _journals.FirstOrDefault(item => item.Id == journalId)
                    ?.Entries.Add(entry);
        }

        public void AddJournalWithChildren(Journal journal)
        {
            throw new NotImplementedException();
        }

        public int AddMood(Mood mood)
        {
            _moods.Add(mood);

            return mood.Id;
        }

        public int AddJournalType(JournalType    journalType)
        {
            _types.Add(journalType);

            return journalType.Id;
        }
        
        public int UpdateMood(Mood mood)
        {
            var oldItem = _moods.FirstOrDefault(arg => arg.Id == mood.Id);
            _moods.Remove(oldItem);
            _moods.Add(mood);

            return mood.Id;
        }

        public int UpdateJournalType(JournalType journalType)
        {
            var oldItem = _types.FirstOrDefault(arg => arg.Id == journalType.Id);
            _types.Remove(oldItem);
            _types.Add(journalType);

            return journalType.Id;
        }

        public void UpdateEntry(Entry entry)
        {
            var oldItem = _entries.FirstOrDefault(arg => arg.Id == entry.Id);
            _entries.Remove(oldItem);
            _entries.Add(entry);
            
        }

        public void UpdateJournal(Journal journal)
        {
            var oldItem = _journals.FirstOrDefault(arg => arg.Id == journal.Id);
            _journals.Remove(oldItem);
            _journals.Add(journal);
            
        }

        public int DeleteMood(ref Mood mood)
        {
            throw new NotImplementedException();
        }

        public int DeleteMood(Mood mood)
        {
            var oldItem = _moods.FirstOrDefault(arg => arg.Id == mood.Id);
            _moods.Remove(oldItem);

            return mood.Id;
        }
        public int DeleteJournalType(ref JournalType journalType)
        {
            throw new NotImplementedException();
        }

        public int DeleteJournal(ref Journal journal
                               , bool        orphanChildren = false)
        {
            return -1;
        }

        public int DeleteJournalType(JournalType journalType)
        {
            var oldItem = _types.FirstOrDefault(arg => arg.Id == journalType.Id);
            _types.Remove(oldItem);

            return journalType.Id;
        }

        public int DeleteEntry(ref Entry entry)
        {
            throw new NotImplementedException();
        }

        public int DeleteEntry(Entry entry)
        {
            var oldItem = _entries.FirstOrDefault(arg => arg.Id == entry.Id);
            _entries.Remove(oldItem);

            return entry.Id;
        }

        public int DeleteJournal(ref Journal journal)
        {
            var oldItem = journal;

            foreach (var entry in oldItem.Entries)
            {
                oldItem.Entries.Remove(entry);
            }
            _journals.Remove(oldItem);

            return journal.Id;
        }

        public Journal GetJournal(int id)
        {
            return _journals.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Journal> GetJournals(bool forceRefresh = false)
        {
            return _journals;
        }

        public Mood GetMood(int id)
        {
            return _moods.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Mood> GetMoods(bool forceRefresh = false)
        {
            return _moods;
        }

        public JournalType GetJournalType(int id)
        {
            return _types.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<JournalType> GetJournalTypes(bool forceRefresh = false)
        {
            return _types;
        }

        public Entry GetEntry(int id)
        {
            return _entries.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Entry> GetEntries(bool forceRefresh = false)
        {
            return _entries;
        }

        public IEnumerable GetEntriesWithMood(int moodId)
        {
            return _entries.Where(fields => fields.EntryMood.Id == moodId);
        }

        Mood IDataStore.AddMood(Mood mood)
        {
            throw new NotImplementedException();
        }
    }
}