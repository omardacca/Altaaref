using Altaaref.Droid;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Identity;
using Android.Gms.Maps;
using Android.Gms.Drive;
using Android.Gms.Auth;
using Android.Gms.Extensions;
using Android.Gms.Auth.Account;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using System.Threading.Tasks;

// https://github.com/xamarin/monodroid-samples/blob/master/FusedLocationProvider/FusedLocationProvider/MainActivity.cs

namespace Altaaref.Droid
{
    [Activity(Label = "Altaaref", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        GoogleApiClient mGoogleApiClient;

        //        GoogleSignInAccount signin = GoogleSignIn.getLastSignedInAccount(this);
        //DriveResourceClient resource;
        //DriveClient drive;

        const long ONE_MINUTE = 60 * 1000;
        const long FIVE_MINUTES = 5 * ONE_MINUTE;
        const long TWO_MINUTES = 2 * ONE_MINUTE;
        const int RC_SIGN_IN = 9001;

        const int RequestLocationId = 0;
        readonly string[] Permissions =
         {
          Manifest.Permission.WriteExternalStorage //, more
         };


        static readonly int RC_LAST_LOCATION_PERMISSION_CHECK = 1000;
        static readonly int RC_LOCATION_UPDATES_PERMISSION_CHECK = 1100;

        static readonly string KEY_REQUESTING_LOCATION_UPDATES = "requesting_location_updates";

        FusedLocationProviderClient _fusedLocationProviderClient;
        LocationCallback locationCallback;
        LocationRequest locationRequest;
        bool isGooglePlayServicesInstalled;
        bool isRequestingLocationUpdates;

        internal static MainActivity Instance { get; private set; }
        public static readonly int DownloadFile = 1000;
        public string Url { get; set; }
        public string Filename { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
    
            Instance = this;

            base.OnCreate(bundle);

            if (bundle != null)
            {
                isRequestingLocationUpdates = bundle.KeySet().Contains(KEY_REQUESTING_LOCATION_UPDATES) &&
                                              bundle.GetBoolean(KEY_REQUESTING_LOCATION_UPDATES);
            }
            else
            {
                isRequestingLocationUpdates = false;
            }

            // Check if Google Services Installed
            isGooglePlayServicesInstalled = IsGooglePlayServicesInstalled();

            if (isGooglePlayServicesInstalled)
            {
                #region Google Location Service
                locationRequest = new LocationRequest()
                                  .SetPriority(LocationRequest.PriorityHighAccuracy)
                                  .SetInterval(FIVE_MINUTES)
                                  .SetFastestInterval(TWO_MINUTES);
                locationCallback = new FusedLocationProviderCallback(this);

                _fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
                #endregion

                #region GoogleSignIn Service

                // [START configure_signin]
                // Configure sign-in to request the user's ID, email address, and basic
                // profile. ID and basic profile are included in DEFAULT_SIGN_IN.
                GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                        .RequestEmail()
                        .Build();
                // [END configure_signin]

                // [START build_client]
                // Build a GoogleApiClient with access to the Google Sign-In API and the
                // options specified by gso.
                mGoogleApiClient = new GoogleApiClient.Builder(this)
                        .EnableAutoManage(this, this)
                        .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                        .Build();
                // [END build_client]

                #endregion
            }
            else
            {
                FinishAndRemoveTask();
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();

            var opr = Auth.GoogleSignInApi.SilentSignIn(mGoogleApiClient);
            if (opr.IsDone)
            {
                // If the user's cached credentials are valid, the OptionalPendingResult will be "done"
                // and the GoogleSignInResult will be available instantly.
                var result = opr.Get() as GoogleSignInResult;
                HandleSignInResult(result);
            }
            else
            {
                // If the user has not previously signed in on this device or the sign-in has expired,
                // this asynchronous branch will attempt to sign in the user silently.  Cross-device
                // single sign-on will occur in this branch.
                opr.SetResultCallback(new SignInResultCallback { Activity = this });
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == DownloadFile)
            {
                StartDownload();
            }

            // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
            if (requestCode == RC_SIGN_IN)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(intent);
                HandleSignInResult(result);
            }
        }

        public void HandleSignInResult(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                // Signed in successfully, show authenticated UI.
                var acct = result.SignInAccount;
                //mStatusTextView.Text = string.Format(GetString(Resource.String.signed_in_fmt), acct.DisplayName);
                //UpdateUI(true);
            }
            else
            {
                // Signed out, show unauthenticated UI.
//                UpdateUI(false);
            }
        }

        void SignIn()
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }

        void SignOut()
        {
            Auth.GoogleSignInApi.SignOut(mGoogleApiClient).SetResultCallback(new SignOutResultCallback { Activity = this });
        }

        void RevokeAccess()
        {
            Auth.GoogleSignInApi.RevokeAccess(mGoogleApiClient).SetResultCallback(new SignOutResultCallback { Activity = this });
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            // An unresolvable error has occurred and Google APIs (including Sign-In) will not
            // be available.
        }

        protected override void OnStop()
        {
            base.OnStop();
            mGoogleApiClient.Disconnect();
        }


        public void SelectFromDrive()
        {
            
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

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK || requestCode == RC_LOCATION_UPDATES_PERMISSION_CHECK)
            {
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    if (requestCode == RC_LAST_LOCATION_PERMISSION_CHECK)
                    {
                        await GetLastLocationFromDevice();
                    }
                    else
                    {
                        await StartRequestingLocationUpdates();
                        isRequestingLocationUpdates = true;
                    }
                }
                else
                {
                    FinishAndRemoveTask();
                    return;
                }
            }
            else if(requestCode == RequestLocationId)
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

                    FinishAndRemoveTask();
                    return;
                }
            }
            else
                Toast.MakeText(this, "Don't know how to handle requestCode " + requestCode, ToastLength.Long).Show();

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void Download()
        {
            Android.Net.Uri contentUri = Android.Net.Uri.Parse(Url);
            Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(contentUri);
            r.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, Filename);
            r.AllowScanningByMediaScanner();
            r.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
            Android.App.DownloadManager dm = (Android.App.DownloadManager)Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.DownloadService);

            dm.Enqueue(r);

            var localFolder = Android.OS.Environment.DirectoryDownloads;
            var MyFilePath = $"file://{localFolder}/{Filename}";
            Toast.MakeText(this, MyFilePath, ToastLength.Long).Show();
        }

        bool IsGooglePlayServicesInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
                return true;

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                // Check if there is a way the user can resolve the issue
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Toast.MakeText(this, string.Format("There is a problem with Google Play Services on this device: {0} - {1}",
                          queryResult, errorString), ToastLength.Long);
            }

            return false;
        }

        public async void GetLastLocationButtonOnClick()
        {
            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted)
            {
                await GetLastLocationFromDevice();
            }
            else
            {
                RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
            }
        }

        //async void RequestLocationUpdatesButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    // No need to request location updates if we're already doing so.
        //    if (isRequestingLocationUpdates)
        //    {
        //        StopRequestionLocationUpdates();
        //        isRequestingLocationUpdates = false;
        //    }
        //    else
        //    {
        //        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
        //        {
        //            await StartRequestingLocationUpdates();
        //            isRequestingLocationUpdates = true;
        //        }
        //        else
        //        {
        //            RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
        //        }
        //    }
        //}

        async Task GetLastLocationFromDevice()
        {
            // This method assumes that the necessary run-time permission checks have succeeded.
            Android.Locations.Location location = await _fusedLocationProviderClient.GetLastLocationAsync();

            if (location == null)
            {
                // Seldom happens, but should code that handles this scenario
            }
            else
            {
                // Do something with the location 
                // Log.Debug("Sample", "The latitude is " + location.Latitude);
                Toast.MakeText(this, "OK, you can use location now, Latitude=" + location.Latitude, ToastLength.Long).Show();
            }
        }

        void RequestLocationPermission(int requestCode)
        {
            if (ShouldShowRequestPermissionRationale(Manifest.Permission.AccessFineLocation))
                RequestPermissions(new[] { Manifest.Permission.AccessFineLocation }, requestCode);
        }

        async Task StartRequestingLocationUpdates()
        {
            await _fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, locationCallback);
        }

        async void StopRequestionLocationUpdates()
        {
            if (isRequestingLocationUpdates)
            {
                await _fusedLocationProviderClient.RemoveLocationUpdatesAsync(locationCallback);
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBoolean(KEY_REQUESTING_LOCATION_UPDATES, isRequestingLocationUpdates);
            base.OnSaveInstanceState(outState);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted)
            {
                if (isRequestingLocationUpdates)
                {
                    await StartRequestingLocationUpdates();
                }
            }
            else
            {
                RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
            }
        }

        protected override void OnPause()
        {
            StopRequestionLocationUpdates();
            base.OnPause();
        }

    }
}

