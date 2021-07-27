using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ThingsToRemember.Models
{
    [Table("Entry")]
    public class Entry
    {
        [PrimaryKey, AutoIncrement]
        public int      Id             { get; set; }
        public string   Title          { get; set; }
        public string   Text           { get; set; }
        public DateTime CreateDateTime { get; set; }
        
        [ForeignKey(typeof(Journal))]
        public int JournalId { get; set; }

        [ForeignKey(typeof(Mood))]
        public int MoodId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Mood EntryMood { get; set; }
    }
}
