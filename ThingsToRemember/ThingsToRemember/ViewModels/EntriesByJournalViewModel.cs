using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class EntriesByJournalViewModel : BaseViewModel
    {
        public IEnumerable<Entry>          Entries                    { get; set; }
        public ObservableCollection<Entry> ObservableListOfEntries    { get; set; }
        public ObservableCollection<Entry> ObservableListOfTtrEntries { get; set; }

        private readonly Journal _journal;

        public EntriesByJournalViewModel(int journalId, bool useMockData = false)
        {
            _journal                   = DataAccessLayer.GetJournal(journalId);

            SetTitle();
            
            Entries                    = GetEntriesByJournal(journalId);

            var entries = Entries.ToList();
            ObservableListOfEntries    = new ObservableCollection<Entry>(entries);

            var ttrEntries = DataAccessLayer.GetEntries()
                                            .Where(fields => fields.IsTtr());
            ObservableListOfTtrEntries = new ObservableCollection<Entry>(ttrEntries);
        }

        private void SetTitle()
        {
            Title = _journal.Title == null ?
                            "TtR Entries" :
                            $"{_journal?.Title} Entries";
        }

        private IEnumerable<Entry> GetEntriesByJournal(int journalId)
        {
            if (_journal.Entries == null)
            {
                return new List<Entry>();
            }
            
            return _journal.Entries;
        }
        
    }
}
