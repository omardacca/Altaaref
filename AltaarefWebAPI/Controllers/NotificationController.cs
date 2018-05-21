using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;

namespace AltaarefWebAPI.Controllers
{
    public class mess
    {
        public string message { get; set; }
        public string templateParam { get; set; }
    }

    [Route("api/Notification")]
    public class NotificationController : Controller
    {
        [HttpPost]
        [Route("topic")]
        public String SendNotificationFromFirebaseCloud()
        {
            var result = "-1";
            var webAddr = "https://fcm.googleapis.com/fcm/send";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization:key=" + "AAAAnO7dP3I:APA91bEfzkmagwS55b1SpnE8YI_Qn8Hks3prHWhtk3x_OTZ6vLyWDpzH8mPMnDkpahGKxU66wuUSWqe0UCvC_Bn6z3tRkSwXKDafhtkZDbmWQt2AjHlz8VbTINN5XqSogzRiFroz58cl");
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{" +
                        "\"message\" : {" +
                        "\"topic\" : \"news\"," +
                        "\"notification\" : {" +
                            "\"body\" : \"This is a Firebase Cloud Messaging Topic Message!\"," +
                                "\"title\" : \"Xamarin University\"," +
                            "\"icon\" : \"myicon\" }" +
                         "}" +
                        "}";


                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationAsync([FromBody] mess message)
        {
            //HttpConfiguration confing = this.Configuration;
            try
            {
                await InternalSendNotificationAsync(message.message, null, message.templateParam);
            }
            catch(Exception ex)
            {
                //confing.Services.GetTraceWriter().Error(ex.Message, null, "Push.SendAsync Error");
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        private async Task<NotificationOutcome> InternalSendNotificationAsync(string message, string installationId, string templateParam)
        {
            //HttpConfiguration config = this.Configuration;
            //var settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            /*
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;
            */

            string notificationHubName = "AltaarefHub";
            string notificationHubConnection = "Endpoint=sb://altaarefnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=pvT/OimyDyTEcI0P1VhXsyN8O+0V+R5WUv/ZkO3Zvug=";

            var hub = NotificationHubClient.CreateClientFromConnectionString(
                notificationHubConnection,
                notificationHubName,
                enableTestSend: true);

            // sending the message so that all template registeration that contain "messageParam" 
            // will receive the notification. This includes APNS, GCM, WNS, and MPNS template registrations.
            var templateParams = new Dictionary<string, string>
            {
                [templateParam] = message
            };

            NotificationOutcome result = null;
            if(string.IsNullOrWhiteSpace(installationId))
            {
                result = await hub.SendTemplateNotificationAsync(templateParams).ConfigureAwait(false);
            }
            else
            {
                result = await hub.SendTemplateNotificationAsync(templateParams, "$InstallationId");
            }

            return result;
        }
    }
}