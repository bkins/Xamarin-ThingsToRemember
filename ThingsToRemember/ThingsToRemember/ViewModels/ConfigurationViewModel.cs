using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        public MoodViewModel        MoodViewModel        { get; set; }
        public JournalTypeViewModel JournalTypeViewModel { get; set; }

        public ConfigurationViewModel()
        {
            MoodViewModel        = new MoodViewModel();
            JournalTypeViewModel = new JournalTypeViewModel();
        }
        public void ClearUserData()
        {
            DataAccessLayer.ResetUserData();
        }

        public void ClearAppData()
        {
            DataAccessLayer.ResetAppData();

            MoodViewModel        = new MoodViewModel();
            JournalTypeViewModel = new JournalTypeViewModel();
        }

        /// <summary>
        /// Problem Journal Type got created without a Title (null).  Since there was not Title, they were not able to be interacted with.
        /// And thus, one was not able to edit or delete them.
        /// Solution: Find all Journals Types with a null Title and delete them.
        /// </summary>
        public void FixJournalTypes()
        {
            var typeCount = DataAccessLayer.GetJournalTypesList()
                                           .Count;
            
            var observableJournalTypes = new ObservableCollection<JournalType>(DataAccessLayer.GetJournalTypesList());

            foreach (var type in observableJournalTypes.Where(type=>type.Title == null))
            {
                var typeToDelete = type;
                DataAccessLayer.DeleteJournalType(ref typeToDelete);
            }

            typeCount = DataAccessLayer.GetJournalTypesList()
                                       .Count;
        }

        /// <summary>
        /// Problem: Some how, presume data was created during an unstable time during development, when Moods were
        /// created but orphaned.  I believe this was during the time when one could created Moods from the Entry page.
        /// This ultimately resulted in Moods that were not assigned to any Entry, nor could they be assigned to an Entry --
        /// probably because of how the Mood picker only will display distinct moods.  This has since been fix (probably).
        /// Solution: Find all Mood that are assigned to an Entry.  Create a list of those Ids.
        /// Find all Moods that have Ids that are not in this list. Loops through these Ids that are not assigned and delete them.
        /// </summary>
        public void CleanupMoods()
        {
            var allEntries          = DataAccessLayer.GetEntries();
            var allAssignedMoodsIds = new List<int>();
            
            foreach (var entry in allEntries)
            {
                if ( ! allAssignedMoodsIds.Contains(entry.MoodId))
                {
                    allAssignedMoodsIds.Add(entry.MoodId);                    
                }
            }

            var allMoodsNotAssigned = DataAccessLayer.GetMoods()
                                                     .Where(fields => ! allAssignedMoodsIds.Contains(fields.Id));
           
            foreach (var mood in allMoodsNotAssigned)
            {
                var moodToDelete = mood;
                DataAccessLayer.DeleteMood(ref moodToDelete);
            }
        }
        
    }
}
