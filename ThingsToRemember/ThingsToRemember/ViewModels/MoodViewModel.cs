using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThingsToRemember.Models;

namespace ThingsToRemember.ViewModels
{
    public class MoodViewModel : BaseViewModel
    {
        public List<string>               AllMoodsForPicker     => GetMoodsForPicker();
        public ObservableCollection<Mood> ObservableListOfMoods { get; set; }
        public Mood                       Mood                  { get; set; }

        public MoodViewModel()
        {
            Mood = new Mood();
            RefreshListOfMoods();
        }

        public MoodViewModel(int moodId)
        {
            Mood = DataAccessLayer.GetMood(moodId);
        }

        public MoodViewModel(string moodTitle)
        {
            Mood = DataAccessLayer.GetMood(moodTitle);
        }
        
        public void RefreshListOfMoods()
        {
            ObservableListOfMoods = new ObservableCollection<Mood>(DataAccessLayer.GetMoods());
        }

        private List<string> GetMoodsForPicker()
        {
            var listOfAllMoods = DataAccessLayer.GetMoods();
            var listForPicker  = new List<string>();
                                 //{
                                 //    "<Add New>"
                                 //};

            foreach (var mood in listOfAllMoods.Where(mood => ! listForPicker.Contains(mood.ToStringWithText())))
            {
                listForPicker.Add(mood.ToStringWithText());
            }

            return listForPicker;
        }

        public Mood AddNewMood(string moodTitle
                             , string moodEmoji)
        {
            return DataAccessLayer.AddMood(moodTitle
                                         , moodEmoji);
        }
        
        public string DeleteMood(int index)
        {
            if (index > ObservableListOfMoods.Count - 1)
            {
                return string.Empty;
            }

            //Get the workout to be deleted
            var moodToDelete = ObservableListOfMoods[index];
            
            //Remove the workout from the source list
            ObservableListOfMoods.RemoveAt(index);

            //DeleteMood the Workout from the database
            var deletedMoodMessage = DataAccessLayer.DeleteMood(ref moodToDelete);
            
            RefreshListOfMoods();

            return deletedMoodMessage;
        }

        public Mood GetMoodToEdit(int index)
        {
            return index > ObservableListOfMoods.Count - 1 ?
                           new Mood() :
                           ObservableListOfMoods[index];
        }

        public Mood GetMood(string title
                           , string emoji)
        {
            return DataAccessLayer.GetMood(title
                                         , emoji);
        }

        public void Save(Mood mood)
        {
            try
            {
                DataAccessLayer.SaveMood(mood);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public void Save()
        {
            Save(Mood);
        }
    }
}
