using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationExceptions;
using ThingsToRemember.Models;
using ThingsToRemember.Services;

namespace ThingsToRemember.ViewModels
{
    class JournalsViewModel : BaseViewModel
    {
        public ObservableCollection<Journal> ObservableListOfJournals { get; set; }
        public IEnumerable<Journal>          Journals                 { get; set; }
        
        public JournalsViewModel(bool useMockData = false)
        {
            Title    = "Journals";
            Journals = GetListOfAllJournals();

            RefreshListOfJournals();
        }

        public void RefreshListOfJournals()
        {
            ObservableListOfJournals = new ObservableCollection<Journal>(DataAccessLayer.GetJournals());
        }

        private IEnumerable<Journal> GetListOfAllJournals() => DataAccessLayer.GetJournals();
        
        private void LoadDummyData(bool clearDataFirst = false)
        {
            
            if (clearDataFirst)
            {
                DataAccessLayer.ClearData();
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

        public string Delete(int index)
        {
            if (index > ObservableListOfJournals.Count - 1)
            {
                return string.Empty;
            }

            //Get the workout to be deleted
            var journalToDelete = ObservableListOfJournals[index];
            var journalTitle    = journalToDelete.Title;

            //Remove the workout from the source list
            ObservableListOfJournals.RemoveAt(index);

            //Delete the Workout from the database
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
    }
}
