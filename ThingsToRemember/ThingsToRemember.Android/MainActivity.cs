using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.Content;
using Avails.Xamarin.Interfaces;
using MediaManager;
using ThingsToRemember.Droid.Utilities;
using ActivityCompat = AndroidX.Core.App.ActivityCompat;

namespace ThingsToRemember.Droid
{
    [Activity(Label = "Things To Remember"
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
        //static readonly          int    NOTIFICATION_ID = 1000;
        //static readonly          string CHANNEL_ID      = "location_notification";
        //internal static readonly string COUNT_KEY       = "count";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //ExternalStorageDirectory
            bool isReadonly  = Environment.MediaMountedReadOnly.Equals(Environment.ExternalStorageState);
            bool isWriteable = Environment.MediaMounted.Equals(Environment.ExternalStorageState);
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if ( ! (CheckPermissionGranted(Manifest.Permission.ReadExternalStorage) 
                &&   ! CheckPermissionGranted(Manifest.Permission.WriteExternalStorage)))
                {
                    RequestPermission();
                }
            }

            base.OnCreate(savedInstanceState);
            
            Rg.Plugins.Popup.Popup.Init(this);
            CrossMediaManager.Current.Init();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Forms.DependencyService.Register<IDependencyService, DependencyImplementation>();
            LoadApplication(new App());

            //CreateNotificationChannel();
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
            return ContextCompat.CheckSelfPermission(this, permissions) == Permission.Granted;
        }

        //private void CreateNotificationChannel()
        //{
        //    if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        //    {
        //        // Notification channels are new in API 26 (and not a part of the
        //        // support library). There is no need to create a notification
        //        // channel on older versions of Android.
        //        return;
        //    }

        //    var name        = Resources.GetString(Resource.String.channel_name);
        //    var description = GetString(Resource.String.channel_description);

        //    var channel = new NotificationChannel(CHANNEL_ID
        //                                        , name
        //                                        , NotificationImportance.Default)
        //                  {
        //                      Description = description
        //                  };

        //    var notificationManager = (NotificationManager) GetSystemService(NotificationService);
        //    notificationManager.CreateNotificationChannel(channel);
        //}

    }
}