using System.IO;
using Android.OS;
using Android.Provider;
using Android.Telephony.Mbms;
using Avails.Xamarin.Interfaces;

namespace ThingsToRemember.Droid.Utilities
{
    public class DependencyImplementation : IDependencyService
    {
        public string GetExternalStorage()
        {
            string path;
            path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            path = Xamarin.Essentials.FileSystem.AppDataDirectory;
            
            //Dev1 SdkInt == P (do not use Documents folder)
            //Dev2 SdkInt == R (use Documents folder)
            // path = Build.VERSION.SdkInt >= BuildVersionCodes.M 
            //     ? Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath 
            //     : Environment.ExternalStorageDirectory?.ToString();
            //BENDO: Replace GetExternalStoragePublicDirectory & ExternalStorageDirectory methods.  They have been deprecated.
            path = Build.VERSION.SdkInt >= BuildVersionCodes.R 
                ? Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath 
                : Environment.ExternalStorageDirectory?.ToString();
            
            return path;
        }
    }
}