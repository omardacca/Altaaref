using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Altaaref.Droid;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(LocationProvider))]
namespace Altaaref.Droid
{
    public class LocationProvider : ILocationProvider
    {
        public void getCurrentLocation()
        {
            MainActivity activity = MainActivity.Instance as MainActivity;
            activity.GetLastLocationButtonOnClick();
        }
    }
}