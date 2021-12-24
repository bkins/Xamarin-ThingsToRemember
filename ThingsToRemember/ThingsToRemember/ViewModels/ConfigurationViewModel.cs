using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
