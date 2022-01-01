using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ThingsToRemember.Services;

namespace ThingsToRemember.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public DataAccess DataAccess;
    
        public DataAccess DataAccessLayer
        {
            get => DataAccess = DataAccess ?? new DataAccess(App.Database);
            set => DataAccess = value;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected bool SetProperty<T>(ref T                     backingStore
                                    , T                         value
                                    , [CallerMemberName] string propertyName = ""
                                    , Action                    onChanged    = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
