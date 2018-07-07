using Altaaref.Models;
using FirebaseNet.Messaging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Altaaref.Helpers
{
    public class FCMPushNotificationSender
    {
        public static async Task Send(string topic, string title, string body)
        {
            HttpClient _client = new HttpClient();

            FCMClient client = new FCMClient(App.FCMServerKey);

            var message = new Message()
            {
                To = "/topics/" + topic,
                Notification = new AndroidNotification()
                {
                    Title = title,
                    Body = body
                }
            };

            var result = await client.SendMessageAsync(message);
        }

        public static async Task<UserNotification> AddNotification(UserNotification userNotification)
        {
            var serializedlist = Application.Current.Properties["SerializedUserNotif"] as string;

            List<UserNotification> list = null;

            if (serializedlist != null)
                list = JsonConvert.DeserializeObject<List<UserNotification>>(serializedlist);
            else
                list = new List<UserNotification>();

            HttpClient _client = new HttpClient();
            var postUrl = "https://altaarefapp.azurewebsites.net/api/UserNotifications";

            var content = new StringContent(JsonConvert.SerializeObject(userNotification), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(postUrl, content);

            var insertedRes = await response.Content.ReadAsStringAsync();
            var InsertedUserNotification = JsonConvert.DeserializeObject<UserNotification>(insertedRes);

            if (response.IsSuccessStatusCode)
            {
                list.Add(InsertedUserNotification);
                var newSerializedlist = JsonConvert.SerializeObject(list);
                Application.Current.Properties["SerializedUserNotif"] = newSerializedlist;

                return InsertedUserNotification;
            }
            else
                return null;
        }
    }
}
