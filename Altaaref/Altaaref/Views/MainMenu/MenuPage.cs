using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Altaaref.Views.MainMenu
{
    public class MenuPage : ContentPage
    {
        public Page GetMenuPage()
        {
            var getnotif = GetNotifications();

            var loc = GetLocation();

            return new NavigationPage(new MainMenuPageContent())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.White
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async Task GetLocation()
        {
            var hasPermission = await Utils.CheckPermissions(Permission.Location);

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;

            var position = await locator.GetPositionAsync();

            Application.Current.Properties["Latitude"] = position.Latitude.ToString();
            Application.Current.Properties["Longtitude"] = position.Longitude.ToString();

            await Application.Current.SavePropertiesAsync();

            //Lat = "Latitude: " + position.Latitude.ToString();
            //Long = "Longtitude: " + position.Longitude.ToString();
        }

        private async Task GetNotifications()
        {
            //Busy = true;
            HttpClient _client = new HttpClient();
            string url = "https://altaarefapp.azurewebsites.net/api/UserNotifications/GetByStudentId/" + Settings.StudentId;

            try
            {
                string results = await _client.GetStringAsync(url);
                var list = JsonConvert.DeserializeObject<List<UserNotification>>(results);

                if (list != null && list.Count != 0)
                    Application.Current.Properties["SerializedUserNotif"] = results;
                else
                    Application.Current.Properties["SerializedUserNotif"] = null;

                await Application.Current.SavePropertiesAsync();

                // Subscribe to topics
                foreach (var un in list)
                    DependencyService.Get<IFCMNotificationSubscriber>().Subscribe(un.Topic);

                // Busy = false;

            }
            catch (Exception e)
            {
                Application.Current.Properties["SerializedUserNotif"] = null;
                await Application.Current.SavePropertiesAsync();

                // Busy = false;
            }

            //Busy = false;
        }
    }
}
