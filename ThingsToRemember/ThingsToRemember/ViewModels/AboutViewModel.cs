using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ThingsToRemember.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public int    DatabaseSize           => DataAccessLayer.GetDatabaseSize();
        public int    DatabaseFileSize       => DataAccessLayer.GetSizeFromFileInfo();
        public string DatabaseSizeForDisplay => GetDatabaseSizeForDisplay();

        private const int KB = 1024;

        private string GetDatabaseSizeForDisplay()
        {
            var pageSize = DatabaseSize     / KB;
            var fileSize = DatabaseFileSize / KB;

            return pageSize != fileSize ?
                           $"{pageSize} {nameof(KB)} ({fileSize} {nameof(KB)})" :
                           $"{pageSize} {nameof(KB)}";
        }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(OpenMoreInfoLink);
        }

        private async void OpenMoreInfoLink()
        {
            await Browser.OpenAsync("https://aka.ms/xamarin-quickstart");
        }

        public ICommand OpenWebCommand { get; }
    }
}