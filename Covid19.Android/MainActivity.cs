using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.OneSignal;
using System.IO;
using Xamarin.Essentials;

namespace Covid19.Droid
{
    [Activity(Label = "Covid19", Icon = "@drawable/iconcovid", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        bool permissionStatus = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            OneSignal.Current.StartInit("255da912-725f-46ad-80eb-e2fc3d6505f7")
                  .EndInit();
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (grantResults[grantResults.Length - 1] == 0)
            {
                Covid19.Models.StaticClass.WritePermission = "Granted";
                var pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
                    + "/DCIM/Camera";
                Directory.CreateDirectory(pathToNewFolder);
            }
            else
            {
                Covid19.Models.StaticClass.WritePermission = "Denied";
            }
                
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            //BadgeDrawable.SetBadgeCount(this, menu.GetItem(0), 3);
            return base.OnPrepareOptionsMenu(menu);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            return base.OnCreateOptionsMenu(menu);
        }

    }
}