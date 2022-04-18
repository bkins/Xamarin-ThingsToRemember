using System;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    class EntryViewModel : BaseViewModel
    {
        public Entry    Entry          { get; set; }
        public string   Text           { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Mood     Mood           { get; set; }
        public string   MoodEmoji      => $"{Mood?.Title} {Mood?.Emoji}";

        public EntryViewModel(int entryId
                            , bool useMockData = false)
        {
            Title = "Entry";

            Entry = entryId == 0 ?
                            new Entry() :
                            GetEntry(entryId);
            
            //Bug: Entry not loading
            //BENDO: Consider swapping out the Async Sqlite NET PCL with the Sync version
            //Raising too many problems, and probably won't really gain any value (besides learning)
            SetProperties();
        }

        private void SetProperties()
        {
            Title          = Entry.Title;
            Text           = Entry.Text;
            CreateDateTime = Entry.CreateDateTime;
            Mood           = Entry.EntryMood;
        }

        private Entry GetEntry(int entryId)
        {
            try
            {
                var entry = DataAccessLayer.GetEntry(entryId);
            
                return entry;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        
        /// <summary>
        /// Only for Updating the this.Entry
        /// </summary>
        public void Save()
        {
            DataAccessLayer.SaveEntry(Entry, Entry.JournalId);
        }
        
        /// <summary>
        /// Save any entry to any journal
        /// </summary>
        /// <param name="entry">An Entry</param>
        /// <param name="journalId">A JournalId</param>
        public void Save(Entry entry, int journalId)
        {
            DataAccessLayer.SaveEntry(entry, journalId);
        }
        
        /// <summary>
        /// Update this.Entry with a different JournalId
        /// </summary>
        /// <param name="journalId"></param>
        public void Save(int journalId)
        {
            DataAccessLayer.SaveEntry(Entry, journalId);
        }

        public Mood FindMood(string moodTitle)
        {
            return DataAccessLayer.GetMood(moodTitle);
        }
        
    }
}
