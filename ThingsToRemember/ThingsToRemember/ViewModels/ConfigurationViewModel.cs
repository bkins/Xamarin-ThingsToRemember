using System;
using System.Collections.Generic;
using System.Text;

namespace ThingsToRemember.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        public MoodViewModel MoodViewModel { get; set; }

        public ConfigurationViewModel()
        {
            MoodViewModel = new MoodViewModel();
        }
        public void ClearData()
        {
            DataAccessLayer.ClearData();
            MoodViewModel = new MoodViewModel();
        }
    }
}
