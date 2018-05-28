using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class SettingsPageViewModel
    {
        private readonly IPageService _pageService;


        public ICommand StudyGroupCommand => new Command(async () => await HandleStudyGroupsTap());
        public ICommand NotebooksCommand => new Command(async () => await HandleNotebooksTap());
        public ICommand MutualHelpCommand => new Command(async () => await HandleMutualHelpCourseTap());
        public ICommand MutualHelpFacultyCommand => new Command(async () => await HandleMutualHelpFacultyTap());
        
        public SettingsPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
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

    }
}
