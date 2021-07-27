using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services
{
    public interface IDataStore
    {
        int  AddJournal            (Journal     journal);
        void AddEntryWIthChildren  (Entry       entry,  int journalId);
        void AddJournalWithChildren(Journal     journal);
        int  AddMood               (Mood        mood);
        int  AddJournalType        (JournalType journalType);
        
        //Entries will always be added via the journal (i.e. aJournal.Entries.Add(anEntry))
        //Task<bool> AddEntryAsync          (Entry       entry);
        
        void UpdateJournal    (Journal     journal);
        int UpdateMood        (Mood        mood);
        int UpdateJournalType (JournalType journalType);
        void UpdateEntry      (Entry       entry);
                   
        int DeleteMood        (Mood        mood);
        int DeleteJournalType (JournalType journalType);
        int DeleteJournal     (Journal     journal);
        int DeleteEntry       (Entry       entry);

        Journal                  GetJournal      (int  id);
        IEnumerable<Journal>     GetJournals     (bool forceRefresh = false);
        Mood                     GetMood         (int  id);
        IEnumerable<Mood>        GetMoods        (bool forceRefresh = false);
        JournalType              GetJournalType  (int  id);
        IEnumerable<JournalType> GetJournalTypes (bool forceRefresh = false);
        Entry                    GetEntry        (int  id);
        IEnumerable<Entry>       GetEntries      (bool forceRefresh = false);
    }
}
