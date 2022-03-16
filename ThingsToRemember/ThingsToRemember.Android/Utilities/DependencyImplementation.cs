using Avails.Xamarin.Interfaces;

namespace ThingsToRemember.Droid.Utilities
{
    public class DependencyImplementation : IDependencyService
    {
        public string GetExternalStorage()
        {
            var path = Android.OS.Environment.ExternalStorageDirectory.ToString();
            return path;
        }
    }
}