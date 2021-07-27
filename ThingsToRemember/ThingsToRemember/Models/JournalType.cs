using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ThingsToRemember.Models
{
    [Table("JournalType")]
    public class JournalType
    {
        [PrimaryKey, AutoIncrement]
        public int    Id    { get; set; }
        public string Title { get; set; }
    }
}
