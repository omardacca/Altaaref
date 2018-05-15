using Altaaref.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Altaaref
{
	public partial class App : Application
	{

        public const string NotificationReceivedKey = "NotificationReceived";
        public const string MobileServiceUrl = "https://altaarefapp.azurewebsites.net";
        

        public App ()
		{
			InitializeComponent();

            SetMainPage();
        }

        private async void SetMainPage()
        {
            if(!string.IsNullOrEmpty(Settings.AccessToken))
            {
                await GetStudentId();

                var page = new Views.MainMenu.MenuPage().GetMenuPage();
                NavigationPage.SetHasNavigationBar(page, false);

                MainPage = new NavigationPage(page);
            }
            else
            {
                MainPage = new NavigationPage(new Views.LoginPage());
            }
            
        }

        private async Task GetStudentId()
        {
            HttpClient _client = new HttpClient();
            var url = "https://altaarefapp.azurewebsites.net/api/Students/GetIdentityIdByStdId/" + Settings.Identity;

            string idstring = await _client.GetStringAsync(url);

            Settings.StudentId = int.Parse(idstring);

            
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
