using Altaaref.Helpers;
using Altaaref.Models;
using Altaaref.Views;
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
    public class PersonalCornerViewModel : BaseViewModel
    {
        //public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }
        private IPageService _pageService = new PageService();
        private HttpClient _client = new HttpClient();

        private Student _student;
        public Student Student
        {
            get { return _student; }
            private set
            {
                _student = value;
                OnPropertyChanged(nameof(Student));
            }
        }


        public PersonalCornerViewModel(IPageService pageService)
        {
            _pageService = pageService;

            var std = GetStudent();
        }

        private async Task GetStudent()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Students/GetStudentByIdentity/" + Settings.Identity;

            string content = await _client.GetStringAsync(url);
            var obj = JsonConvert.DeserializeObject<Student>(content);

            Student = obj;
        }

        public ICommand HomePageCommand => new Command(async () => await HandleHomePageTap());
        public ICommand MyStudyGroupsCommand => new Command(async () => await HandleStudyGroupsTap());
        public ICommand StudyGroupsInvitationsCommand => new Command(async () => await HandleStudyGroupsInvitationTap());
        public ICommand FavoriteNotebooksCommand => new Command(async () => await HandleFavoriteNotebooksTap());
        public ICommand MyNotebooksCommand => new Command(async () => await HandleMyNotebooksTap());
        public ICommand MyHelpRequestsCommand => new Command(async () => await HandleMyHelpRequestsTap());
        public ICommand SettingsCommand => new Command(async () => await HandleSettingsTap());
        public ICommand SignOutCommand => new Command(async () => await HandleSignOutTap());
        public ICommand ViewMyRidesInvitations => new Command(async () => await HandleViewMyRidesInv());


        async Task HandleHomePageTap()
        {
            await _pageService.PopAsync();
        }
        async Task HandleStudyGroupsTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.MyStudyGroupsPage());
        }
        async Task HandleStudyGroupsInvitationTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.ViewStudyGroupInvitations());
        }
        async Task HandleFavoriteNotebooksTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.ViewFavoriteNotebooks());
        }
        async Task HandleMyNotebooksTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.MyNotebooksList());
        }
        async Task HandleMyHelpRequestsTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.MyHelpRequests());
        }
        async Task HandleSettingsTap()
        {
            await _pageService.PushAsync(new Views.CommonPages.SettingsPages.SettingsMainPage());
        }
        async Task HandleSignOutTap()
        {
            await _pageService.PushAsync(new Views.LoginPage(LoginPage.LOGOUT_CODE));
        }
        async Task HandleViewMyRidesInv()
        {
            await _pageService.PushAsync(new Views.CommonPages.MyRidesRequests());
        }

    }
}
