using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThingsToRemember.Models;
using ThingsToRemember.Services;

namespace ThingsToRemember.ViewModels
{
    public class EntriesByJournalViewModel : BaseViewModel
    {
        public IEnumerable<Entry> Entries { get; set; }

        private IDataStore _dataStore;

        public EntriesByJournalViewModel(int journalId, bool useMockData = false)
        {
                //LoadDummyData();
                        
            Entries = GetEntriesByJournal(journalId);

        }

        private IEnumerable<Entry> GetEntriesByJournal(int journalId)
        {
            try
            {
                var journal = App.Database.GetJournal(journalId);
            
                return journal.Entries;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        
    }
}
