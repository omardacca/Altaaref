using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
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
                    Application.Current.Properties.Add("UserNotifications", list);
                else
                    Application.Current.Properties.Add("UserNotifications", null);

                // Busy = false;

            }
            catch (Exception e)
            {
                Application.Current.Properties.Add("UserNotifications", null);
                // Busy = false;
            }

            //Busy = false;
        }
    }
}
