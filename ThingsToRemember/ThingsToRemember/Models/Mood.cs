using SQLite;
using Xamarin.Forms;

namespace ThingsToRemember.Models
{
    [Table("Mood")]
    public class Mood
    {
        [PrimaryKey, AutoIncrement]
        public int    Id    { get; set; }
        public string Title { get; set; }
        public string Emoji { get; set; }
        
        public override string ToString()
        {
            return $"{Emoji}";
        }

        public string ToStringWithText()
        {
            return $"{Title} {ToString()}";
        }
    }
}
