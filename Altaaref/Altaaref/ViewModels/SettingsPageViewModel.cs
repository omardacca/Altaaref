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
            string url = "https://altaarefapp.azurewebsites.net/api/UserNotifications/GetGeneralHR/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            var results = JsonConvert.DeserializeObject<UserNotification>(content);

            if (results == null) IsGeneralToggled = false;
            else IsGeneralToggled = true;

            // Busy = false;
        }

        async Task<bool> DeleteGeneralHR()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/UserNotifications/DeleteGeneralHR/" + Settings.StudentId;

            var response = await _client.DeleteAsync(postUrl);

            return response.IsSuccessStatusCode;
        }

        async Task HandleGeneralHRToggle()
        {
            await DeleteGeneralHR();
        }


    }
}
