using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    class EntryViewModel : BaseViewModel
    {
        public     Entry       Entry          { get; set; }
        public new string      Title          { get; set; }
        public     string      Text           { get; set; }
        public     DateTime    CreateDateTime { get; set; }
        public     Mood        Mood           { get; set; }

        public EntryViewModel(int entryId, bool useMockData = false)
        {
            //LoadDummyData();
            
            Entry = GetEntry(entryId);
            
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
                var entry = App.Database.GetEntry(entryId);
            
                return entry;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
    }
}
