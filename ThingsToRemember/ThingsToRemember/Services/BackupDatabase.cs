using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Extensions;
using ThingsToRemember.Models;

namespace ThingsToRemember.Services;

public class BackupDatabase : IDataStore
{
    private readonly SQLiteConnection _database;
    private readonly string           _path;
    private readonly Database         _sourceDatabase;
    public BackupDatabase (string path, Database sourceDatabase)
    {
        _database       = new SQLiteConnection(path);
        _path           = path;
        _sourceDatabase = sourceDatabase;
        
        CreateUserTables();
        CreateAppTables();
    }
    
    
    public string GetFilePath()
    {
        return _path;
    }

    public void CreateUserTables()
    {
        _database.CreateTable<Journal>();
        _database.CreateTable<Entry>();
    }

    public void CreateAppTables()
    {
        _database.CreateTable<Mood>();
        _database.CreateTable<JournalType>();
    }

    public void BackupDatabaseFromSource()
    {
        foreach (var mood in _sourceDatabase.GetMoods())
        {
            _database.Insert(mood);
        }

        foreach (var journalType in _sourceDatabase.GetJournalTypes())
        {
            _database.Insert(journalType);
        }
        
        foreach (var journal in _sourceDatabase.GetJournals())
        {
            _database.Insert(journal);
        }
        
        foreach (var entry in _sourceDatabase.GetEntries())
        {
            _database.Insert(entry);
        }
    }

    public void RestoreDatabaseFromDestination()
    {
        _sourceDatabase.ResetAllData();
        
        foreach (var mood in GetMoods())
        {
            _sourceDatabase.AddMood(mood);
        }

        foreach (var journalType in GetJournalTypes())
        {
            _sourceDatabase.AddJournalType(journalType);
        }
        
        foreach (var journal in GetJournals())
        {
            _sourceDatabase.AddJournal(journal);
        }
        
        foreach (var entry in GetEntries())
        {
            _sourceDatabase.AddEntry(entry);
        }
    }
    
    public int AddJournal(Journal journal) => throw new System.NotImplementedException();

    public void AddEntry(Entry entry
                       , int   journalId)
    {
        throw new System.NotImplementedException();
    }

    public void AddEntryWIthChildren(Entry entry
                                   , int   journalId)
    {
        throw new System.NotImplementedException();
    }

    public void AddJournalWithChildren(Journal   journal)
    {
        throw new System.NotImplementedException();
    }

    public Mood AddMood(Mood                     mood) => throw new System.NotImplementedException();

    public int AddJournalType(JournalType        journalType) => throw new System.NotImplementedException();

    public void UpdateJournal(Journal            journal)
    {
        throw new System.NotImplementedException();
    }

    public int UpdateMood(Mood                   mood) => throw new System.NotImplementedException();

    public int UpdateJournalType(JournalType     journalType) => throw new System.NotImplementedException();

    public void UpdateEntry(Entry                entry)
    {
        throw new System.NotImplementedException();
    }

    public void SaveJournalType(JournalType      journalType)
    {
        throw new System.NotImplementedException();
    }

    public void SaveJournal(Journal              journal)
    {
        throw new System.NotImplementedException();
    }

    public void SaveMood(Mood                    mood)
    {
        throw new System.NotImplementedException();
    }

    public int DeleteMood(ref        Mood        mood) => throw new System.NotImplementedException();

    public int DeleteJournalType(ref JournalType journalType) => throw new System.NotImplementedException();

    public int DeleteJournal(ref Journal journal
                           , bool        orphanChildren = false) => throw new System.NotImplementedException();

    public int DeleteEntry(ref Entry                     entry) => throw new System.NotImplementedException();

    public int DeleteEntry(Entry                         entry) => throw new System.NotImplementedException();

    public Journal GetJournal(int                        id) => throw new System.NotImplementedException();

    public IEnumerable<Journal> GetJournals(bool forceRefresh = false)
    {
        return _database.GetAllWithChildren<Journal>();
    }

    public Mood GetMood(int                              id) => throw new System.NotImplementedException();

    public IEnumerable<Mood> GetMoods(bool forceRefresh = false)
    {
        return _database.GetAllWithChildren<Mood>();
    }

    public JournalType GetJournalType(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<JournalType> GetJournalTypes(bool forceRefresh = false)
    {
        return _database.GetAllWithChildren<JournalType>();
    }

    public Entry GetEntry(int                            id) => throw new System.NotImplementedException();

    public IEnumerable<Entry> GetEntries(bool forceRefresh = false)
    {
        return _database.GetAllWithChildren<Entry>();
    }

    public IEnumerable GetEntriesWithMood(int            moodId) => throw new System.NotImplementedException();

    public IEnumerable GetJournalsWithJournalTYpe(int    journalTypeId) => throw new System.NotImplementedException();

    public void DropUserTables()
    {
        throw new System.NotImplementedException();
    }

    public void DropAppTables()
    {
        throw new System.NotImplementedException();
    }

    public int GetSizeFromPageCountByPageSize() => throw new System.NotImplementedException();

    public int GetSizeFromFileInfo() => throw new System.NotImplementedException();

    public void Close()
    {
        throw new System.NotImplementedException();
    }

    public string GetFileName() => throw new System.NotImplementedException();

    
}