using Altaaref.Helpers;
using Altaaref.Models;
using Altaaref.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        //public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            //ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            //public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }
            private IPageService pageService = new PageService();

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

            public MainPageMasterViewModel()
            {
                var std = GetStudent();
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion

            private async Task GetStudent()
            {
                HttpClient _client = new HttpClient();
                string url = "https://altaarefapp.azurewebsites.net/api/Students/GetStudentByIdentity/" + Settings.Identity;

                string content = await _client.GetStringAsync(url);
                var obj = JsonConvert.DeserializeObject<Student>(content);

                Student = obj;
            }

            public ICommand HomePageCommand => new Command(async() => await HandleHomePageTap());
            public ICommand MyStudyGroupsCommand => new Command(async () => await HandleStudyGroupsTap());
            public ICommand StudyGroupsInvitationsCommand => new Command(async () => await HandleStudyGroupsInvitationTap());
            public ICommand FavoriteNotebooksCommand => new Command(async () => await HandleFavoriteNotebooksTap());
            public ICommand MyNotebooksCommand => new Command(async () => await HandleMyNotebooksTap());
            public ICommand MyHelpRequestsCommand => new Command(async () => await HandleMyHelpRequestsTap());
            public ICommand SettingsCommand => new Command(async () => await HandleSettingsTap());
            public ICommand SignOutCommand => new Command(async () => await HandleSignOutTap());


            async Task HandleHomePageTap()
            {
                await pageService.PushAsync(new Views.CommonPages.MainPage());
            }
            async Task HandleStudyGroupsTap()
            {
                await pageService.PushAsync(new Views.CommonPages.MyStudyGroupsPage());
            }
            async Task HandleStudyGroupsInvitationTap()
            {
                await pageService.PushAsync(new Views.CommonPages.ViewStudyGroupInvitations());
            }
            async Task HandleFavoriteNotebooksTap()
            {
                await pageService.PushAsync(new Views.CommonPages.ViewFavoriteNotebooks());
            }
            async Task HandleMyNotebooksTap()
            {
                await pageService.PushAsync(new Views.CommonPages.MyNotebooksList());
            }
            async Task HandleMyHelpRequestsTap()
            {
                await pageService.PushAsync(new Views.CommonPages.MyHelpRequests());
            }
            async Task HandleSettingsTap()
            {
                await pageService.PushAsync(new Views.CommonPages.MainPage());
            }
            async Task HandleSignOutTap()
            {
                await pageService.PushAsync(new Views.LoginPage(LoginPage.LOGOUT_CODE));
            }



        }
    }
}