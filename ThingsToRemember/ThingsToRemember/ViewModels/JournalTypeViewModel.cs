using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class JournalTypeViewModel : BaseViewModel
    {
        //public IList<JournalType> JournalTypes => GetListOfJournalsTypesForPicker();
        public IList<string>      JournalTypes => GetListOfJournalsTypesForPicker();

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
    }
}
