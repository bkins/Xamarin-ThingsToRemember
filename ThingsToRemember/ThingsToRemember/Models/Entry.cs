using System;
using Avails.D_Flat;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ThingsToRemember.Models
{
    [Table("Entry")]
    public class Entry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get;                     set; }
        public string   Title             { get; set; }
        public string   Text              { get; set; }
        public DateTime CreateDateTime    { get; set; }
        public byte[]   Image             { get; set; }
        public string   ImageFileName     { get; set; }
        public byte[]   Video             { get; set; }
        public string   VideoFileName     { get; set; }
        public int      OriginalJournalId { get; set; }
        
        [ForeignKey(typeof(Journal))]
        public int JournalId { get; set; }

        [ForeignKey(typeof(Mood))]
        public int MoodId { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public Mood EntryMood { get; set; }

        public Entry()
        {
            CreateDateTime = DateTime.Now;
            Image          = Array.Empty<byte>();
            ImageFileName  = string.Empty;
        }

        public bool IsTtr(DateTime dateTimeNow)
        {
            return CreateDateTime.IsDateOnPassedInDate(dateTimeNow);  //.IsDateOnThisDayInThePast();

            //BENDO:  THis is logic is duplicated in the JournalsViewModel as well.  Make in only one place!
            //return CreateDateTime.Date  <  DateTime.Today
            //    && CreateDateTime.Month == DateTime.Today.Month 
            //    && CreateDateTime.Day   == DateTime.Today.Day;
        }
    }
}
