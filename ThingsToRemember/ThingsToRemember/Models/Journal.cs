using System;
using System.Collections.Generic;
using System.Text;
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
        
        [ForeignKey(typeof(JournalType))]
        public int JournalTypeId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public JournalType JournalType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Entry> Entries { get; set; }
    }
    
}
