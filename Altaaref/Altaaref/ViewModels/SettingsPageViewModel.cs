using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly IPageService _pageService;
        private HttpClient _client = new HttpClient();


        public ICommand StudyGroupCommand => new Command(async () => await HandleStudyGroupsTap());
        public ICommand NotebooksCommand => new Command(async () => await HandleNotebooksTap());
        public ICommand MutualHelpCommand => new Command(async () => await HandleMutualHelpCourseTap());
        public ICommand MutualHelpFacultyCommand => new Command(async () => await HandleMutualHelpFacultyTap());
        public ICommand ToggleCommand => new Command(async () => await HandleGeneralHRToggle());

        private bool _isGeneralToggled;
        public bool IsGeneralToggled
        {
            get { return _isGeneralToggled; }
            set
            {
                SetValue(ref _isGeneralToggled, value);
            }
        }

        public SettingsPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            var generaltoggle = GetGeneralHRStatus();
        }

        async Task HandleStudyGroupsTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.SettingsPages.NotebookNotificationsSettings(NotificationSettingsViewModelType.StudyGroup));
        }

        async Task HandleNotebooksTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.SettingsPages.NotebookNotificationsSettings(NotificationSettingsViewModelType.NotebooksStorage));
        }

        async Task HandleMutualHelpCourseTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.SettingsPages.NotebookNotificationsSettings(NotificationSettingsViewModelType.MutualHelpCourse));
        }

        async Task HandleMutualHelpFacultyTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.SettingsPages.NotificationSettingsFacultyBased(NotificationSettingsViewModelType.MutualHelpFaculty));
        }

        async Task GetGeneralHRStatus()
        {
            // Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/UserNotifications/GetGeneralByStudentId/" + Settings.StudentId;

            try
            {
                string content = await _client.GetStringAsync(url);

                var results = JsonConvert.DeserializeObject<List<UserNotification>>(content);

                if (results.Count == 0) IsGeneralToggled = false;
                else IsGeneralToggled = true;
            }
            catch(Exception e)
            {

            }

            // Busy = false;
        }

        async Task PostGeneralHRStatus()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/UserNotifications/GeneralHR";

            var newGeneral = new UserNotification
            {
                StudentId = Settings.StudentId,
                Title = "Help!",
                Body = "Student is calling for help, check it out.",
                Topic = "HRGeneral"
            };

            var content = new StringContent(JsonConvert.SerializeObject(newGeneral), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var newInserted = JsonConvert.DeserializeObject<UserNotification>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                // Subscribe to topic
                DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("HRGeneral");

                IsGeneralToggled = true;

                
            }
        }

        async Task<bool> DeleteGeneralHR()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/UserNotifications/DeleteGeneralHR/" + Settings.StudentId;

            var response = await _client.DeleteAsync(postUrl);

            if (response.IsSuccessStatusCode)
            {
                DependencyService.Get<IFCMNotificationSubscriber>().UnSubscribe("GE" + Settings.StudentId);
                IsGeneralToggled = false;
            }
            return response.IsSuccessStatusCode;
        }

        public async Task HandleGeneralHRToggle()
        {
            if (IsGeneralToggled)
                await PostGeneralHRStatus();
            else
                await DeleteGeneralHR();
        }


    }
}
