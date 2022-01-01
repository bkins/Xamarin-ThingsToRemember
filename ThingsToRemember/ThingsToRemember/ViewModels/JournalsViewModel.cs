using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avails.D_Flat;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class JournalsViewModel : BaseViewModel
    {
        public ObservableCollection<Journal> ObservableListOfJournals { get; set; }
        public IEnumerable<Journal>          Journals                 { get; set; }
        
        public JournalsViewModel(bool useMockData = false)
        {
            Title = "Things to Remember";
            //Title    = "Journals";
            Journals = GetListOfAllJournals();

            RefreshListOfJournals();
        }

        public void RefreshListOfJournals()
        {
            Journals = DataAccessLayer.GetJournals()
                                      .ToList();

            //SetExtensionProperties();

            ObservableListOfJournals = new ObservableCollection<Journal>(Journals);
        }

        //private void SetExtensionProperties()
        //{
        //    foreach (var journal in Journals)
        //    {
        //        SetEntryCount(journal);

        //        SetThingsToRememberCount(journal);
        //    }
        //}

        //private static void SetThingsToRememberCount(Journal journal)
        //{
        //    var thingsToRememberCount = journal.Entries.Count(fields => fields.IsTtr());

        //    journal.HasEntriesToRemember = thingsToRememberCount != 0;

        //    if (journal.HasEntriesToRemember)
        //    {
        //        journal.HasEntriesToRememberText = $"TtR: {thingsToRememberCount}";
        //    }
        //}

        //private static void SetEntryCount(Journal journal)
        //{
        //    var entryCount = journal.Entries.Count;

        //    if (entryCount != 0)
        //    {
        //        journal.EntryCountText = $"Entries: {entryCount}";
        //    }
        //}

        private IEnumerable<Journal> GetListOfAllJournals() => DataAccessLayer.GetJournals();
        
        private void LoadDummyData(bool clearDataFirst = false)
        {
            
            if (clearDataFirst)
            {
                DataAccessLayer.ResetUserData();
                
            }
            
            var expectedJournalTypeTitle        = "Test Journal Type";
            var expectedAnotherJournalTypeTitle = "Another Test Journal Type";
            var expectedMoodHappy               = "Happy";
            var expectedMoodSad                 = "Sad";
            var expectedMoodMad                 = "Mad";
            var expectedMoodMeh                 = "Meh";
            var expectedEntry1Title             = "Entry 1";
            var expectedEntry2Title             = "Entry 2";
            var expectedEntry3Title             = "Entry 3";
            var expectedEntryNTitle             = "Entry n";
            var expectedJournalTitle            = "Test Journal";

            var journalType = new JournalType()
                              {
                                  Title = expectedJournalTypeTitle
                              };
            
            //App.Database.SaveJournalType(journalType);

            var anotherJournalType = new JournalType()
                                     {
                                         Title = expectedAnotherJournalTypeTitle
                                     };

            DataAccessLayer.SaveJournalType(anotherJournalType);
            

            var moodHappy = new Mood()
                            {
                                Title = expectedMoodHappy
                              , Emoji = ":)"
                            };

            var moodSad = new Mood()
                          {
                              Title = expectedMoodSad
                            , Emoji = ":("
                          };
            
            var moodMad = new Mood()
                          {
                              Title = expectedMoodMad
                            , Emoji = ">:("
                          };
            
            var moodMeh = new Mood()
                          {
                              Title = expectedMoodMeh
                            , Emoji = ":|"
                          };

            DataAccessLayer.SaveMood(moodHappy);
            DataAccessLayer.SaveMood(moodSad);
            DataAccessLayer.SaveMood(moodMad);
            DataAccessLayer.SaveMood(moodMeh);

            var entry1 = new Entry()
                         {
                             Title          = expectedEntry1Title
                           , Text           = "Test entry"
                           , CreateDateTime = DateTime.Now
                           , EntryMood      = moodHappy
                           , MoodId         = moodHappy.Id
                         };

            var entry2 = new Entry()
                         {
                             Title          = expectedEntry2Title
                           , CreateDateTime = DateTime.Now
                           , EntryMood      = moodSad
                         };

            var entry3 = new Entry()
                         {
                             Title          = expectedEntry3Title
                           , CreateDateTime = DateTime.Now
                           , EntryMood      = moodMad
                         };

            var entries = new List<Entry>
                          {
                              entry1
                            , entry2
                            , entry3
                          };

            var journal = new Journal()
                          {
                              Title       = expectedJournalTitle
                            , JournalType = journalType
                            , Entries     = new List<Entry>()
                          };

            foreach (var entry in entries)
            {
                journal.Entries.Add(entry);
            }

            //var journal = new Journal()
            //              {
            //                  Title       = expectedJournalTitle
            //                , JournalType = journalType
            //                , Entries     = entries
            //              };

            var lastMinuteEntry = new Entry()
                                  {
                                      Title          = expectedEntryNTitle
                                    , Text           = "Entry to be added to a journal that has already been add to the DB."
                                    , CreateDateTime = DateTime.Now
                                    , EntryMood      = moodMeh
                                  };
            
            //Act: Add

            DataAccessLayer.AddJournalWithChildren(journal);

            DataAccessLayer.SaveEntry(entry1
                                    , journal.Id);
            
            DataAccessLayer.SaveEntry(entry2
                                    , journal.Id);

            DataAccessLayer.SaveEntry(entry3
                                    , journal.Id);

            journal.Entries.Add(lastMinuteEntry);

            DataAccessLayer.SaveEntry(lastMinuteEntry
                                    , journal.Id);

            
        }

        public Journal GetJournal(int index)
        {
            return ObservableListOfJournals[index];
        }

        public string Delete(int index, Journal journalToDelete)
        {
            if (index > ObservableListOfJournals.Count - 1)
            {
                return string.Empty;
            }
            
            var journalTitle    = journalToDelete.Title;
            
            ObservableListOfJournals.RemoveAt(index);
            
            DataAccessLayer.DeleteJournal(ref journalToDelete);
            
            RefreshListOfJournals();

            return journalTitle;
        }

        public Journal GetJournalToEdit(int index)
        {
            return index > ObservableListOfJournals.Count - 1 ?
                           new Journal() :
                           ObservableListOfJournals[index];
        }

        public void Save(Journal journal)
        {
            try
            {
                DataAccessLayer.SaveJournal(journal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public bool AnyWithTtr()
        {
            return Journals.Any(fields => fields.HasEntriesToRemember);

            //return DataAccessLayer.GetJournals()
            //                      .Any(fields => fields.Entries.Any() 
            //                                  && fields.Entries.Any(entries => entries.CreateDateTime.Date  >  DateTime.Today
            //                                                                && entries.CreateDateTime.Month == DateTime.Today.Month 
            //                                                                && entries.CreateDateTime.Day   == DateTime.Today.Day));
        }
    }
}
