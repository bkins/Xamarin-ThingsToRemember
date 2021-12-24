using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services
{
    public interface IDataStore
    {
        int  AddJournal            (Journal journal);
        void AddEntryWIthChildren  (Entry   entry, int journalId);
        void AddJournalWithChildren(Journal journal);
        Mood AddMood               (Mood    mood);
        int  AddJournalType        (JournalType     journalType);
        
        //Entries will always be added via the journal (i.e. aJournal.Entries.Add(anEntry))
        //Task<bool> AddEntryAsync          (Entry       entry);
        
        void UpdateJournal    (Journal     journal);
        int UpdateMood        (Mood        mood);
        int UpdateJournalType (JournalType journalType);
        void UpdateEntry      (Entry       entry);
                   
        int DeleteMood        (ref Mood        mood);
        int DeleteJournalType (ref JournalType journalType);
        int DeleteJournal     (ref Journal     journal, bool orphanChildren = false);
        int DeleteEntry       (ref Entry       entry);
        int DeleteEntry       (    Entry       entry);

        Journal                  GetJournal                 (int    id);
        IEnumerable<Journal>     GetJournals                (bool   forceRefresh = false);
        Mood                     GetMood                    (int    id);
        IEnumerable<Mood>        GetMoods                   (bool   forceRefresh = false);
        JournalType              GetJournalType             (int    id);
        IEnumerable<JournalType> GetJournalTypes            (bool   forceRefresh = false);
        Entry                    GetEntry                   (int    id);
        IEnumerable<Entry>       GetEntries                 (bool   forceRefresh = false);
        IEnumerable              GetEntriesWithMood         (int    moodId);
        IEnumerable              GetJournalsWithJournalTYpe (int    journalTypeId);
    }
}
