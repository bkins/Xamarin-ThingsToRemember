using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Attributes;
using ThingsToRemember.Services;

namespace ThingsToRemember.Models
{
    [Table("Journal")]
    public class Journal : JournalExtended
    {
        [PrimaryKey, AutoIncrement]
        public int    Id     { get; set; }
        public string Title  { get; set; }

        #region Ignored Properties
        
        [Ignore]
        private DateTime DateTimeNow { get; set; }

        [Ignore]
        public int EntriesToRememberCount => GetEntriesToRememberCount(DateTimeNow);
        public bool HasEntriesToRemember => GetHasEntriesToRemember(DateTimeNow);

        [Ignore]
        public int EntriesCount => GetEntriesCount();

        private int GetEntriesCount()
        {
            return Entries.Count();
        }

        public int GetEntriesToRememberCount(DateTime dateTimeNow)
        {
            return Entries.Count(fields => fields.IsTtr(dateTimeNow));
        }

        public bool GetHasEntriesToRemember(DateTime dateTimeNow)
        {
            return JournalType.Title != SystemJournalGenerator.SystemJournalTypeTitle 
                && Entries.Any(fields => fields.IsTtr(dateTimeNow));
        }

        #endregion

        [ForeignKey(typeof(JournalType))]
        public int JournalTypeId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public JournalType JournalType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Entry> Entries { get; set; }

        public Journal()
        {
            DateTimeNow = DateTime.Now;
        }

        public Journal(DateTime dateTimeNow)
        {
            DateTimeNow = dateTimeNow;
        }

        public override string ToString()
        {
            return $"{Title} ({Id})";
        }
    }
    
}
