using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.HelpRequest
{
    public class MainPageHelpRequestViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        private bool _generalListEmpty;
        public bool GeneralListEmpty
        {
            get { return _generalListEmpty; }
            set
            {
                SetValue(ref _generalListEmpty, value);
            }
        }

        private bool _facultiesListEmpty;
        public bool FacultiesListEmpty
        {
            get { return _facultiesListEmpty; }
            set
            {
                SetValue(ref _facultiesListEmpty, value);
            }
        }

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

        private List<StudentHelpRequest> _generalHelpRequestList;
        public List<StudentHelpRequest> GeneralHelpRequestList
        {
            get { return _generalHelpRequestList; }
            private set
            {
                _generalHelpRequestList = value;
                OnPropertyChanged(nameof(GeneralHelpRequestList));
            }
        }

        private List<FacultyHelpRequest> _facultiesHelpRequestsList;
        public List<FacultyHelpRequest> FacultiesHelpRequestsList
        {
            get { return _facultiesHelpRequestsList; }
            private set
            {
                _facultiesHelpRequestsList = value;
                OnPropertyChanged(nameof(FacultiesHelpRequestsList));
            }
        }

        public ICommand ViewCommand => new Command<StudentHelpRequest>(HandleHRTap); // changed
        public ICommand GeneralViewAllCommand => new Command(HandleGeneralViewAll);
        public ICommand FacultiesViewAllCommand => new Command(HandleFacultiesViewAll);

        public MainPageHelpRequestViewModel(IPageService pageService)
        {
            _pageService = pageService;

            var tasks = InitLists();

        }

        private async Task InitLists()
        {
            await GetStudentInfo();
            await GetGeneralHelpRequest();
            await GetNotGeneralHelpRequest();
        }


        private void HandleGeneralViewAll()
        {
            
        }

        private void HandleFacultiesViewAll()
        {

        }

        private async void HandleHRTap(StudentHelpRequest studentHelpRequest)
        {
            await _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }

        public async Task GetStudentInfo()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Students/204228043";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<Student>(content);

            Student = list;
        }

        public async Task GetGeneralHelpRequest()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/General";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            if (list == null || list.Count == 0)
                GeneralListEmpty = true;
            else
                GeneralListEmpty = false;

            GeneralHelpRequestList = new List<StudentHelpRequest>(list);
        }

        public async Task GetNotGeneralHelpRequest()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpFaculties/GetStudentFacultiesHR/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<FacultyHelpRequest>>(content);

            if (list == null || list.Count == 0)
                FacultiesListEmpty = true;
            else
                FacultiesListEmpty = false;

            FacultiesHelpRequestsList = new List<FacultyHelpRequest>(list);
        }

        //public async void GetNotGeneralWithFacultyIdHelpRequest()
        //{
        //    string url = "https://altaarefapp.azurewebsites.net/api/HelpFaculties/" + facultyId + "/" + restrictDate.Date.Date;

        //    string content = await _client.GetStringAsync(url);
        //    var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

        //    HelpRequestsList = new List<StudentHelpRequest>(list);
        //}


    }
}
