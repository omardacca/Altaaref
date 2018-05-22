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
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Droid;
using Firebase.Iid;
using System;
using Firebase.Messaging;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using Lottie.Forms.Droid;
using System.IO;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Altaaref.Droid
{
    [Activity(Label = "Altaaref", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        GoogleApiClient mGoogleApiClient;

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
        public static readonly int UPLOAD_CODE = 123;
        public static readonly int SAVETO_CODE = 124;

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

        public int CourseId { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }


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

                FirebaseMessaging.Instance.SubscribeToTopic("news");
            }
            else
            {
                FinishAndRemoveTask();
            }

            global::Xamarin.Forms.Forms.Init(this, bundle);

            ImageCircleRenderer.Init();

            AnimationViewRenderer.Init();

            LoadApplication(new App());

#if DEBUG
            // Force refresh of the token. If we redeploy the app, no new token will be sent but the old one will
            // be invalid.
            Task.Run(() =>
            {
                // This may not be executed on the main thread.
                FirebaseInstanceId.Instance.DeleteInstanceId();
                //Console.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
            });
#endif

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

        private string GetRealPathFromUri(Android.Net.Uri contentUri)
        {
            Android.Database.ICursor cursor = ContentResolver.Query(contentUri, null, null, null, null);
            cursor.MoveToFirst();
            string docId = cursor.GetString(0);
            docId = docId.Substring(docId.LastIndexOf(":") + 1);
            cursor.Close();

            cursor = ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null,
                Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { docId }, null);

            cursor.MoveToFirst();

            string path = cursor.GetString(cursor.GetColumnIndex(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

        private bool IsDocumentUri(Android.Net.Uri uri)
        {
            bool ret = false;
            if (uri != null)
                ret = Android.Provider.DocumentsContract.IsDocumentUri(this, uri);
            return ret;
        }

        private bool IsDownloadDoc(string uriAuthority)
        {
            bool ret = false;

            if ("com.android.providers.downloads.documents".Equals(uriAuthority))
                ret = true;
            return ret;
        }

        protected async override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if(requestCode == UPLOAD_CODE)
            {
                if (IsDocumentUri(intent.Data))
                {
                    string documentId = Android.Provider.DocumentsContract.GetDocumentId(intent.Data);
                    string uriAuthority = intent.Data.Authority;

                    if (IsDownloadDoc(uriAuthority))
                    {
                        Android.Net.Uri downloadUri = Android.Net.Uri.Parse("content://downloads/public_downloads");

                        Android.Net.Uri downloadUriAppendId = ContentUris.WithAppendedId(downloadUri, long.Parse(documentId));

                        Java.IO.File file = new Java.IO.File(downloadUriAppendId.ToString());

                        Stream inputStream = ContentResolver.OpenInputStream(downloadUriAppendId);

                        string blobUrl = await UploadFileToBlob(inputStream);

                        AddNotebook(blobUrl);
                    }
                }

            }
            
            if(requestCode == SAVETO_CODE)
            {
                Intent sendIntent = new Intent();
                sendIntent.SetType("application/pdf");

                //sendIntent.SetAction(Intent.ActionSend);
                sendIntent.SetAction(Intent.ActionSend);
                if(IsDocumentUri(intent.Data))
                {
                    string documentId = Android.Provider.DocumentsContract.GetDocumentId(intent.Data);
                    string uriAuthority = intent.Data.Authority;

                    if(IsDownloadDoc(uriAuthority))
                    {
                        Android.Net.Uri downloadUri = Android.Net.Uri.Parse("content://downloads/public_downloads");

                        Android.Net.Uri downloadUriAppendId = ContentUris.WithAppendedId(downloadUri, long.Parse(documentId));

                        sendIntent.SetData(downloadUriAppendId);
                        sendIntent.PutExtra(Android.Content.Intent.ExtraStream, downloadUriAppendId);

                        StartActivity(Android.Content.Intent.CreateChooser(sendIntent, "Save"));

                    }
                }
            }


            // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
            if (requestCode == RC_SIGN_IN)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(intent);
                HandleSignInResult(result);
            }
        }

        public class Notebook
        {
            public string Name { get; set; }
            public string BlobURL { get; set; }
            public int CourseId { get; set; }
            public int StudentId { get; set; }
        }

        private void AddNotebook(string blobUrl)
        {
            HttpClient _client = new HttpClient();
            var postUrl = "https://altaarefapp.azurewebsites.net/api/Notebooks";

            Notebook newNotebook = new Notebook
            {
                CourseId = CourseId,
                Name = Name,
                BlobURL = blobUrl,
                StudentId = StudentId
            };

            var content = new StringContent(JsonConvert.SerializeObject(newNotebook), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Notebook Added Successfully", ToastLength.Short);
            }
            else
            {
                Toast.MakeText(this, "Notebook not added, something went wrong!", ToastLength.Short);
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

        public void StartDownload(string url, string filename)
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                Download(url, filename);
                return;
            }

            GetStoragePermission();
        }

        private void GetStoragePermission()
        {
            const string permission = Manifest.Permission.WriteExternalStorage;
            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                Download(Url, Filename);
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

        public async Task<string> UploadFileToBlob(Stream path)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=csb08eb270fff55x4a98xb1a;AccountKey=7ROeIOcZq54z+OnYRzR+YJow+sSu3ElALl/HCxjX/LaGLQy6eDY8Ij/E1aFNC4v1ls0SUHPteDzkU1cBzrPpXw==;EndpointSuffix=core.windows.net");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("notebooks");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            // Retrieve reference to a blob named "filename".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + ".pdf");

            // Create the "filename" blob with the text "Hello, world!"
            await blockBlob.UploadFromStreamAsync(path);

            return blockBlob.Uri.AbsoluteUri;
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
                    Download(Url, Filename);
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
        
        private void Download(string Url, string filename)
        {
            Android.Net.Uri contentUri = Android.Net.Uri.Parse(Url);
            Android.App.DownloadManager.Request r = new Android.App.DownloadManager.Request(contentUri);
            r.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads, filename);
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

        // This service handles the device's registration with FCM.
        [Service]
        [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
        public class MyFirebaseIIDService : FirebaseInstanceIdService
        {
            public override async void OnTokenRefresh()
            {
                var refreshedToken = FirebaseInstanceId.Instance.Token;
                //Console.WriteLine($"Token received: {refreshedToken}");
                await SendRegistrationToServerAsync(refreshedToken);
            }

            async Task SendRegistrationToServerAsync(string token)
            {
                try
                {
                    // Formats: https://firebase.google.com/docs/cloud-messaging/concept-options
                    // The "notification" format will automatically displayed in the notification center if the 
                    // app is not in the foreground.
                    const string templateBodyFCM =
                        "{" +
                            "\"notification\" : {" +
                            "\"body\" : \"$(messageParam)\"," +
                                "\"title\" : \"Xamarin University\"," +
                            "\"icon\" : \"myicon\" }" +
                        "}";

                    const string templateTopicFCM =
                        "{" +
                        "\"message\" : {" +
                        "\"topic\" : \"news\"," +
                        "\"notification\" : {" +
                            "\"body\" : \"$(messageParam)\"," +
                                "\"title\" : \"Xamarin University\"," +
                            "\"icon\" : \"myicon\" }" +
                         "}" +
                        "}";

                    var templates = new JObject();
                    templates["genericMessage"] = new JObject
                    {
                        {"body", templateBodyFCM},
                    };

                    var topictemplates = new JObject();
                    topictemplates["messageParam"] = new JObject
                    {
                        {"body", templateTopicFCM }
                    };

                    var client = new MobileServiceClient(Altaaref.App.MobileServiceUrl);
                    var push = client.GetPush();

                    await push.RegisterAsync(token, templates);
                    await push.RegisterAsync(token, topictemplates);

                    // Push object contains installation ID afterwards.
                    //Console.WriteLine(push.InstallationId.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Debugger.Break();
                }
            }
        }

        // This service is used if app is in the foreground and a message is received.
        [Service]
        [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
        public class MyFirebaseMessagingService : FirebaseMessagingService
        {
            public override void OnMessageReceived(RemoteMessage message)
            {
                base.OnMessageReceived(message);

                //Console.WriteLine("Received: " + message);

                // Android supports different message payloads. To use the code below it must be something like this (you can paste this into Azure test send window):
                // {
                //   "notification" : {
                //      "body" : "The body",
                //                 "title" : "The title",
                //                 "icon" : "myicon
                //   }
                // }
                try
                {
                    var msg = message.GetNotification().Body;
                    MessagingCenter.Send<object, string>(this, Altaaref.App.NotificationReceivedKey, msg);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error extracting message: " + ex);
                }
            }
        }
    }
}


