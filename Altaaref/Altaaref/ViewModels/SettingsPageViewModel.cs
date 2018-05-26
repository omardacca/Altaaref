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
        public ICommand MutualHelpCommand => new Command(async () => await HandleMutualHelpTap());

        public SettingsPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        async Task HandleStudyGroupsTap()
        {
            //_pageService.PushAsync(new Views.CommonPages.)
        }

        async Task HandleNotebooksTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.SettingsPages.NotebookNotificationsSettings());
        }

        async Task HandleMutualHelpTap()
        {
            //_pageService.PushAsync(new Views.CommonPages.)
        }

    }
}
