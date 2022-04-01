using System.Collections.Generic;
using System.Linq;
using ThingsToRemember.Models;
using ThingsToRemember.ViewModels;

namespace ThingsToRemember.Services;

public static class SystemJournalGenerator
{
    private static DataAccess _dataAccess;
    private static DataAccess DataAccessLayer
    {
        get => _dataAccess ??= new DataAccess(App.Database);
        set => _dataAccess = value;
    }

    public static  string       Trash                  { get => "Trash"; }
    public static  string       Archive                { get => "Archive"; } 
    private static List<string> SystemJournalTitles    { get => new() { Archive, Trash }; }
    public static  string       SystemJournalTypeTitle { get => "System"; }

    public static List<Journal> SystemJournals { get;  set; }

    public static void Generate ()
    {
        var systemJournalType      = GetSystemJournalType();
        var existingSystemJournals = DataAccessLayer.GetJournals()
                                                    .ToList();
        
        foreach (var journalViewModel in 
                        from title in SystemJournalTitles
                        where ! existingSystemJournals.Any() ||
                              SystemJournalDoesNotExist(existingSystemJournals
                                                      , title)
                        select new JournalViewModel(title
                                                  , systemJournalType.Title))
        {
            journalViewModel.Save();
        }
    }

    private static bool SystemJournalDoesNotExist(List<Journal> existingSystemJournals
                                                , string title)
    {
        return ! existingSystemJournals.Any(journal=> journal != null 
                                                   && journal.Title == title 
                                                   && journal.JournalType.Title == SystemJournalTypeTitle);
    }

    private static JournalType GetSystemJournalType()
    {
        var systemJournalType = DataAccessLayer.GetJournalType(SystemJournalTypeTitle);

        if (systemJournalType != null)
            return systemJournalType;

        //Add System Journal Type
        DataAccessLayer.AddJournalType(new JournalType { Title = SystemJournalTypeTitle });
        systemJournalType = _dataAccess.GetJournalType(SystemJournalTypeTitle);

        return systemJournalType;
    }
}