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

        public void StartDownload(string url, string filename)
        {
            // Get the MainActivity instance
            MainActivity activity = MainActivity.Instance as MainActivity;

            activity.Url = url;
            activity.Filename = filename;

            activity.StartDownload(url, filename);

        }

        public void SaveTo()
        {
            // Get the MainActivity instance
            MainActivity activity = MainActivity.Instance as MainActivity;

            Intent viewIntent = new Intent();
            viewIntent.SetType("application/pdf");
            viewIntent.SetAction(Intent.ActionGetContent);

            activity.StartActivityForResult(viewIntent, MainActivity.SAVETO_CODE);
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