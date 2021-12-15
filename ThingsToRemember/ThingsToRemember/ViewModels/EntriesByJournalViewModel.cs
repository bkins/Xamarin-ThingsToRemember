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
    public class EntriesByJournalViewModel : BaseViewModel
    {
        public IEnumerable<Entry>          Entries                 { get; set; }
        public ObservableCollection<Entry> ObservableListOfEntries { get; set; }

        private readonly Journal _journal;

        public EntriesByJournalViewModel(int journalId, bool useMockData = false)
        {
            _journal                = DataAccessLayer.GetJournal(journalId);
            Title                   = $"{_journal.Title} Entries";
            Entries                 = GetEntriesByJournal(journalId);
            ObservableListOfEntries = new ObservableCollection<Entry>(Entries);
        }

        private IEnumerable<Entry> GetEntriesByJournal(int journalId)
        {
            try
            {
                return _journal.Entries;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        
    }
}
