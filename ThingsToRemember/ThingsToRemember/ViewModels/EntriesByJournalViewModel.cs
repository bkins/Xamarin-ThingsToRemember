using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class EntriesByJournalViewModel : BaseViewModel
    {
        public  IEnumerable<Entry>          Entries                 { get; private set; }
        private ObservableCollection<Entry> _observableListOfEntries { get; set; }
        private bool                        _forTtRs                 { get; }
        
        private Journal                     _journal;

        public EntriesByJournalViewModel(int      journalId
                                       , DateTime dateTimeNow
                                       , bool     forTtRs
                                       , bool     useMockData = false)
        {
            _journal = DataAccessLayer.GetJournal(journalId);

            SetTitle();

            _forTtRs = forTtRs;

            Entries = GetEntriesByJournal(dateTimeNow);

            LoadAppropriateEntries(dateTimeNow);
        }

        private void LoadAppropriateEntries(DateTime dateTimeNow)
        {
            if (_forTtRs)
            {
                Entries = DataAccessLayer.GetEntries()
                                         .Where(fields => fields.IsTtr(dateTimeNow));

                _observableListOfEntries = new ObservableCollection<Entry>(Entries);
            }
            else
            {
                Entries = GetEntriesByJournal(dateTimeNow
                                            , forceRefresh: true);
                
                _observableListOfEntries = new ObservableCollection<Entry>(Entries.ToList());
            }
        }

        private void SetTitle()
        {
            Title = _forTtRs ?
                        "TtR Entries" :
                        $"{_journal?.Title} Entries";
        }

        private IEnumerable<Entry> GetEntriesByJournal(DateTime dateTimeNow
                                                     , bool forceRefresh = false)
        {
            if (_forTtRs 
             && ! forceRefresh)
            {
                LoadAppropriateEntries(dateTimeNow);

                return Entries;
            }
            
            if (_journal.Entries == null)
            {
                return new List<Entry>();
            }

            if (forceRefresh)
            {
                _journal = DataAccessLayer.GetJournal(_journal.Id);
            }
            
            return _journal.Entries.OrderByDescending(entry=>entry.CreateDateTime);
        }

        public string Delete(Entry entryToDelete, DateTime dateTimeNow)
        {
            var entryTitle = entryToDelete.Title;

            _observableListOfEntries.Remove(entryToDelete);
            
            DataAccessLayer.DeleteEntry(ref entryToDelete);
            
            RefreshListOfEntries(dateTimeNow);

            return entryTitle;
        }

        public void RefreshListOfEntries(DateTime dateTimeNow)
        {
            Entries = GetEntriesByJournal(dateTimeNow, forceRefresh: true);
            
            LoadAppropriateEntries(dateTimeNow);
        }

        public void MoveEntry(Entry swipedItem
                            , int newJournalId)
        {
            swipedItem.JournalId = newJournalId;
            DataAccessLayer.SaveEntry(swipedItem, newJournalId);
        }

        public object GetObservableEntries(DateTime dateTimeNow)
        {
            if(_observableListOfEntries is null)
            {
                LoadAppropriateEntries(dateTimeNow);
            }
            return _observableListOfEntries.OrderByDescending(fields => fields.CreateDateTime);
        }

        public Entry[] GetArrayOfEntries()
        {
            return Entries.ToArray();
        }

        public void DeleteAll(DateTime dateTimeNow)
        {
            foreach (var entry in Entries)
            {
                var entryToDelete = entry;
                DataAccessLayer.DeleteEntry(ref entryToDelete);
                
            }
                //BENDO: It appears RefreshListOfEntries is not refreshing the list after delete
                RefreshListOfEntries(dateTimeNow);
        }
    }
}
