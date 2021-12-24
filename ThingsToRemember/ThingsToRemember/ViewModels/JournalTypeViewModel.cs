using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class JournalTypeViewModel : BaseViewModel
    {
        //public IList<JournalType> JournalTypes => GetListOfJournalsTypesForPicker();
        public IList<string>                     JournalTypes           => GetListOfJournalsTypesForPicker();
        public ObservableCollection<JournalType> ObservableJournalTypes { get; set; }
        public JournalType                       JournalType            { get; set; }

        public JournalTypeViewModel()
        {
            JournalType = new JournalType();
            RefreshListOfJournalTypes();
        }
        
        public JournalTypeViewModel(int journalTypeId)
        {
            JournalType = DataAccessLayer.GetJournalType(journalTypeId);
        }

        public void RefreshListOfJournalTypes()
        {
            ObservableJournalTypes = new ObservableCollection<JournalType>(DataAccessLayer.GetJournalTypesList());
        }

        private List<string> GetListOfJournalsTypesForPicker()
        {
            //ICollection<MyClass> withoutDuplicates = new HashSet<MyClass>(inputList);
            //List<JournalType> types = DataAccessLayer.GetJournalTypesList();
            List<string> types = DataAccessLayer.GetJournalTypeNameList();
            //types.Add(new JournalType("<Add New>"));
            types.Add("<Add New>");

            //return types.OrderBy(fields=>fields.Title).ToList();
            types.Sort();
            return types;
        }

        public void AddJournalType(JournalType journalType)
        {
            DataAccessLayer.AddJournalType(journalType);
        }

        public JournalType FindJournalType(string typeTitle)
        {
            return DataAccessLayer.GetJournalType(typeTitle);
        }

        public void Save(JournalType journalType)
        {
            DataAccessLayer.SaveJournalType(journalType);
        }

        public void Save()
        {
            DataAccessLayer.SaveJournalType(JournalType);
        }

        public string Delete(int index)
        {
            if (index > ObservableJournalTypes.Count - 1)
            {
                return string.Empty;
            }
            
            //Get the workout to be deleted
            var journalTypeToDelete = ObservableJournalTypes[index];
            
            //Remove the workout from the source list
            ObservableJournalTypes.RemoveAt(index);

            //DeleteMood the Workout from the database
            var deletedMoodMessage = DataAccessLayer.DeleteJournalType(ref journalTypeToDelete);
            
            RefreshListOfJournalTypes();

            return deletedMoodMessage;
        }

        public JournalType GetJournalTypeToEdit(int index)
        {
            return index > ObservableJournalTypes.Count - 1 ?
                           new JournalType() :
                           ObservableJournalTypes[index];
        }
    }
}
