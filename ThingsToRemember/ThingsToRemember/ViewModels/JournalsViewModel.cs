using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingsToRemember.Models;
using ThingsToRemember.Services;

namespace ThingsToRemember.ViewModels
{
    class JournalsViewModel : BaseViewModel
    {
        public IEnumerable<Journal> Journals { get; set; }

        private IDataStore           _dataStore;

        public JournalsViewModel(bool useMockData = false)
        {
            LoadDummyData(true);
            Journals = GetListOfAllJournals();
        }

        private IEnumerable<Journal> GetListOfAllJournals() => App.Database.GetJournals();

        
        private static void LoadDummyData(bool clearDataFirst = false)
        {
            if (clearDataFirst)
            {
                App.Database.DropTables();
                App.Database.CreateTables();
            }

            var expectedJournalTypeTitle = "Test Journal Type";
            var expectedMoodHappy        = "Happy";
            var expectedMoodSad          = "Sad";
            var expectedMoodMad          = "Mad";
            var expectedMoodMeh          = "Meh";
            var expectedEntry1Title      = "Entry 1";
            var expectedEntry2Title      = "Entry 2";
            var expectedEntry3Title      = "Entry 3";
            var expectedEntryNTitle      = "Entry n";
            var expectedJournalTitle     = "Test Journal";

            var journalType = new JournalType()
                              {
                                  Title = expectedJournalTypeTitle
                              };
            
            App.Database.SaveJournalType(journalType);

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

            App.Database.SaveMood(moodHappy);
            App.Database.SaveMood(moodSad);
            App.Database.SaveMood(moodMad);
            App.Database.SaveMood(moodMeh);

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

            App.Database.AddJournalWithChildren(journal);

            App.Database.SaveEntry(entry1, journal.Id);
            App.Database.SaveEntry(entry2, journal.Id);
            App.Database.SaveEntry(entry3, journal.Id);

            journal.Entries.Add(lastMinuteEntry);
            App.Database.SaveEntry(lastMinuteEntry, journal.Id);
            
            
        }

    }
}
