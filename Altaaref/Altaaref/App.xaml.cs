using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Altaaref
{
	public partial class App : Application
	{

        public const string NotificationReceivedKey = "NotificationReceived";
        public const string MobileServiceUrl = "https://altaarefapp.azurewebsites.net";
        

        //public static IAuthenticate Authenticator { get; private set; }

        //public static void Init(IAuthenticate authenticator)
        //{
        //    Authenticator = authenticator;
        //}

        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new Views.CommonPages.MainPage());
            //MainPage = new NavigationPage(new Views.CommonPages.MainPage());
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
