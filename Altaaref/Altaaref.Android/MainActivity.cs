using Altaaref.Droid;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Plugin.Permissions;
using System;
using System.Net;

namespace Altaaref.Droid
{
    [Activity(Label = "Altaaref", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        public static readonly int DownloadFile = 1000;
        public string url { get; set; }
        public string filename { get; set; }


        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Instance = this;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }



        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == DownloadFile)
            {
                StartDownload();
            }
        }

        private void StartDownload()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                Download();
                return;
            }

            GetStoragePermission();
        }

        private void GetStoragePermission()
        {
            const string permission = Manifest.Permission.WriteExternalStorage;
            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                Download();
                return;
            }

            // need to request permission
            if (ShouldShowRequestPermissionRationale(permission))
            {

                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("explain");
                callDialog.SetMessage("Permission to Write on local storage is needed");
                callDialog.SetNeutralButton("yes", delegate
                {
                    RequestPermissions(Permissions, RequestLocationId);
                });
                callDialog.SetNegativeButton("no", delegate { });

                callDialog.Show();
                return;
            }
            //Finally request permissions with the list of permissions and Id
            RequestPermissions(Permissions, RequestLocationId);
        }

        const int RequestLocationId = 0;
        readonly string[] Permissions =
         {
          Manifest.Permission.WriteExternalStorage //, more
         };


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission granted
                            Download();
                        }
                        else
                        {
                            //Permission Denied
                            Toast.MakeText(this, "Permission Request Denied", ToastLength.Long).Show();
                        }
                    }
                    break;
            }
        }

        private void Download()
        {
            Android.Net.Uri contentUri = Android.Net.Uri.Parse(url);
            Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(contentUri);
            r.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, filename);
            r.AllowScanningByMediaScanner();
            r.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
            Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

            dm.Enqueue(r);

            var localFolder = Android.OS.Environment.DirectoryDownloads;
            //var MyFilePath = System.IO.Path.Combine(localFolder, filename);
            var MyFilePath = $"file://{localFolder}/{filename}";
            Toast.MakeText(this, MyFilePath, ToastLength.Long).Show();
        }

    }
}

