using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class EntriesByJournalViewModel : BaseViewModel
    {
        public  IEnumerable<Entry>          Entries                    { get; set; }
        private IEnumerable<Entry>          ttrEntries                 { get; set; }
        private ObservableCollection<Entry> ObservableListOfEntries    { get; set; }
        private ObservableCollection<Entry> ObservableListOfTtrEntries { get; set; }
        public  bool                        ForTtRs                    { get; set; }
        
        private Journal                     _journal;

        public EntriesByJournalViewModel(int      journalId
                                       , DateTime dateTimeNow
                                       , bool     forTtRs
                                       , bool     useMockData = false)
        {
            _journal = DataAccessLayer.GetJournal(journalId);

            SetTitle();

            ForTtRs = forTtRs;

            Entries = GetEntriesByJournal(dateTimeNow);

            LoadAppropriateEntries(dateTimeNow);
        }

        private void LoadAppropriateEntries(DateTime dateTimeNow)
        {
            if (ForTtRs)
            {
                Entries = DataAccessLayer.GetEntries()
                                         .Where(fields => fields.IsTtr(dateTimeNow));

                ObservableListOfEntries = new ObservableCollection<Entry>(Entries);
            }
            else
            {
                Entries = GetEntriesByJournal(dateTimeNow
                                            , forceRefresh: true);
                
                ObservableListOfEntries = new ObservableCollection<Entry>(Entries.ToList());
            }
        }

        private void SetTitle()
        {
            Title = _journal.Title == null ?
                            "TtR Entries" :
                            $"{_journal?.Title} Entries";
        }

        private IEnumerable<Entry> GetEntriesByJournal(DateTime dateTimeNow
                                                     , bool forceRefresh = false)
        {
            if (ForTtRs 
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

        // public string Delete(int   index
        //                    , Entry entryToDelete)
        public string Delete(Entry entryToDelete, DateTime dateTimeNow)
        {
            // if (index > ObservableListOfEntries.Count - 1)
            // {
            //     return string.Empty;
            // }
            
            var entryTitle = entryToDelete.Title;

            ObservableListOfEntries.Remove(entryToDelete);
            // ObservableListOfEntries.RemoveAt(index);
            
            DataAccessLayer.DeleteEntry(ref entryToDelete);
            
            RefreshListOfEntries(dateTimeNow);

            return entryTitle;
        }

        public void RefreshListOfEntries(DateTime dateTimeNow)
        {
            // Entries = DataAccessLayer.GetEntries()
            //                          .ToList();
            
            Entries = GetEntriesByJournal(dateTimeNow, forceRefresh: true);
            
            //SetExtensionProperties();
            LoadAppropriateEntries(dateTimeNow);
            // ObservableListOfEntries = new ObservableCollection<Entry>(Entries);
        }

        public void MoveEntry(Entry swipedItem
                             , int newJournalId)
        {
            swipedItem.JournalId = newJournalId;
            DataAccessLayer.SaveEntry(swipedItem, newJournalId);
        }

        public object GetObservableEntries(bool isTtR)
        {
            return ObservableListOfEntries.OrderByDescending(fields => fields.CreateDateTime);
        }

        public Entry[] GetArrayOfEntries()
        {
            return Entries.ToArray();

            // var ttrArray = ttrEntries.ToArray();
            // var array    = Entries.ToArray();
            //
            // return isTtR ?
            //     ttrArray :
            //     array;


        }
    }
}
