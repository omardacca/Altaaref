using System;
using System.IO;
using System.Net.Http;
using Altaaref.Droid;
using Android;
using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Widget;
using Android.Content;
using Xamarin.Forms;
using Android.Support.V4.Content;

[assembly: Xamarin.Forms.Dependency(typeof(Downloader))]
namespace Altaaref.Droid
{
    public class Downloader : IDownloader
    {

        public string Url { get; set; }
        public string Filename { get; set; }

        public void StartDownload(string url, string filename)
        {
            this.Url = url;
            this.Filename = filename;

            // Get the MainActivity instance
            MainActivity activity = MainActivity.Instance as MainActivity;

            Intent viewIntent = new Intent();
            viewIntent.SetType("application/pdf");
            viewIntent.SetAction(Intent.ActionGetContent);
            
            //viewIntent.SetAction(Intent.ActionGetContent);

            activity.StartActivityForResult(viewIntent, 123);
            //            activity.StartActivityForResult(Android.Content.Intent.CreateChooser(viewIntent, "Share"), 123);

            //Intent intent = new Intent();
            //intent.SetType("application/pdf");
            ////            intent.SetAction(Intent.ActionSend);
            //intent.SetAction(Intent.ActionSend);
            //intent.PutExtra(Android.Content.Intent.ExtraStream, "thisisfile");

            //activity.Url = url;
            //activity.Filename = filename;

            //activity.StartActivity(Android.Content.Intent.CreateChooser(intent, "Share"));
            //activity.StartActivityForResult(Intent.CreateChooser(intent, "Download File"), MainActivity.DownloadFile);
        }


        //private async Task<MemoryStream> getStreamAsync()
        //{
        //    var stream = new MemoryStream();
        //    using (var httpClient = new HttpClient())
        //    {
        //        var downloadStream = await httpClient.GetStreamAsync(new Uri(url));
        //        if (downloadStream != null)
        //        {
        //            await downloadStream.CopyToAsync(stream);
        //        }
        //    }

        //    return stream;
        //}

    }
}