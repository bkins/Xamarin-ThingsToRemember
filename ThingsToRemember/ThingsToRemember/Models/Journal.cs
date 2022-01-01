using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Attributes;

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
        public int    EntryCount     => Entries.Count;
        [Ignore]
        public string EntryCountText => $"Entries: {EntryCount}";

        [Ignore]
        public int EntriesToRememberCount => Entries.Count(fields => fields.IsTtr());
        [Ignore]
        public string HasEntriesToRememberText => $"TtR: {EntriesToRememberCount}";
        [Ignore]
        public bool   HasEntriesToRemember     => Entries.Any(fields => fields.IsTtr());

        #endregion

        [ForeignKey(typeof(JournalType))]
        public int JournalTypeId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public JournalType JournalType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Entry> Entries { get; set; }
        
    }
    
}
