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
        public ObservableCollection<string>  JournalsToMoveTo         { get; set; }
        public int                           EntryCount               { get; set; }
        public int                           EntriesToRememberCount   { get; set; }
        public string                        JournalIdToExclude       { get; set; }

        public JournalsViewModel(bool useMockData = false)
        {
            Title = "Things to Remember";
            //Journals = GetListOfAllJournals();
            JournalIdToExclude = "0";

            RefreshListOfJournals();

        }

        public JournalsViewModel(string excludedJournalId)
        {
            Title = "Things to Remember";
            //Journals = GetListOfAllJournals();
            
            JournalIdToExclude = excludedJournalId;
            
            RefreshListOfJournals();
        }

        public void RefreshListOfJournals()
        {
            Journals = DataAccessLayer.GetJournals()
                                      .ToList();
            JournalsToMoveTo = GetJournalsToMove();
            
            //SetExtensionProperties();

            ObservableListOfJournals = new ObservableCollection<Journal>(Journals);
            
        }

        public ObservableCollection<string> GetJournalsToMove()
        {
            return new ObservableCollection<string>(Journals.Where(fields=>fields.Id != int.Parse(JournalIdToExclude))
                                                            .Select(fields => fields.ToString()));
        }

        private IEnumerable<Journal> GetListOfAllJournals() => DataAccessLayer.GetJournals();
        
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

        public bool AnyWithTtr(DateTime dateTimeNow)
        {
            bool anyWithTtr = Journals.Any(journal => journal.GetHasEntriesToRemember(dateTimeNow));
            return anyWithTtr;
        }
    }
}
