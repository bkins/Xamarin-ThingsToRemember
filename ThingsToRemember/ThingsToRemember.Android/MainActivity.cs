using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using MediaManager;
using MediaManager.Forms.Platforms.Android;

namespace ThingsToRemember.Droid
{
    [Activity(Label = "ThingsToRemember"
            , Icon = "@mipmap/icon"
            , Theme = "@style/MainTheme"
            , MainLauncher = true
            , ConfigurationChanges = ConfigChanges.ScreenSize 
                                   | ConfigChanges.Orientation 
                                   | ConfigChanges.UiMode 
                                   | ConfigChanges.ScreenLayout 
                                   | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!(CheckPermissionGranted(Manifest.Permission.ReadExternalStorage) 
                &&  !CheckPermissionGranted(Manifest.Permission.WriteExternalStorage)))
                {
                    RequestPermission();
                }
            }
            base.OnCreate(savedInstanceState);
            
            Rg.Plugins.Popup.Popup.Init(this);
            CrossMediaManager.Current.Init();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        private void RequestPermission()
        {
            ActivityCompat.RequestPermissions(this
                                            , new[]
                                              {
                                                  Manifest.Permission.ReadExternalStorage
                                                , Manifest.Permission.WriteExternalStorage
                                              }
                                            , 0);
        }

        public bool CheckPermissionGranted(string permissions)
        {
            // Check if the permission is already available.
            if (ContextCompat.CheckSelfPermission(this, permissions) != Permission.Granted)
            {
                return false;
            }

            return true;
        }

        public override void OnRequestPermissionsResult(int                          requestCode
                                                      , string[]                     permissions
                                                      , [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}