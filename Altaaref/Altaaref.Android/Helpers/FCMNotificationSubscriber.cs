using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Altaaref.Droid.Helpers;
using Altaaref.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;

[assembly: Xamarin.Forms.Dependency(typeof(FCMNotificationSubscriber))]
namespace Altaaref.Droid.Helpers
{
    public class FCMNotificationSubscriber : IFCMNotificationSubscriber
    {
        public void Subscribe(string topic)
        {
            FirebaseMessaging.Instance.SubscribeToTopic(topic);
        }
    }
}