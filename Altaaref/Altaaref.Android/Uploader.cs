using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altaaref.Droid;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(Uploader))]
namespace Altaaref.Droid
{
    public class Uploader : IUploader
    {
        public async Task UploadToBlob(int CourseId, string Name, int StudentId, bool IsPrivate)
        {
            // Get the MainActivity instance
            MainActivity activity = MainActivity.Instance as MainActivity;

            Intent viewIntent = new Intent();
            viewIntent.SetType("application/pdf");
            viewIntent.SetAction(Intent.ActionGetContent);

            activity.CourseId = CourseId;
            activity.Name = Name;
            activity.StudentId = StudentId;
            activity.IsPrivate = IsPrivate;

            activity.StartActivityForResult(viewIntent, MainActivity.UPLOAD_CODE);
        }

    }
}