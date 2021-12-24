using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ThingsToRemember.Models
{
    public class JournalExtended
    {
        [Ignore]
        public string EntryCount           { get; set; }
        [Ignore]
        public string HasEntriesToRemember { get; set; }
    }
}
