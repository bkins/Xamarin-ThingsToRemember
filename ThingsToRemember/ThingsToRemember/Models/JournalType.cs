using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ThingsToRemember.Models
{
    [Table("JournalType")]
    public class JournalType
    {
        public JournalType() { }

        public JournalType(string title)
        {
            Title = title;
        }

        [PrimaryKey, AutoIncrement]
        public int    Id    { get; set; }
        public string Title { get; set; }

        
        public override string ToString()
        {
            return Title;
        }
    }
}
