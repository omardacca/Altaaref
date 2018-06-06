using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Altaaref.ViewModels
{
    public class StudentProfileViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<Models.HelpRequest> _helpRequestsList;
        public List<Models.HelpRequest> HelpRequestsList
        {
            get
            {
                return _helpRequestsList;
            }
            set
            {
                _helpRequestsList = value;
                OnPropertyChanged(nameof(HelpRequestsList));
            }
        }

        private List<Models.StudyGroup> _studyGroupList;
        public List<Models.StudyGroup> StudyGroupsList
        {
            get { return _studyGroupList; }
            set
            {
                _studyGroupList = value;
                OnPropertyChanged(nameof(StudyGroupsList));
            }
        }

        private List<Models.Notebook> _notebooksList;
        public List<Models.Notebook> NotebooksList
        {
            get { return _notebooksList; }
            set
            {
                _notebooksList = value;
                OnPropertyChanged(nameof(NotebooksList));
            }
        }

        private List<Ride> _ridesList;
        public List<Ride> RidesList
        {
            get { return _ridesList; }
            set
            {
                _ridesList = value;
                OnPropertyChanged(nameof(_ridesList));
            }
        }

        private Student _studentProfiel;
        public Student StudentProfile
        {
            get { return _studentProfiel; }
            set
            {
                _studentProfiel = value;
                OnPropertyChanged(nameof(StudentProfile));
            }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        public StudentProfileViewModel(IPageService pageService, int StudentId)
        {
            _pageService = pageService;


        }

        private async Task InitProfileAsync(int StudentId)
        {
            Busy = true;

            await GetStudentDetails(StudentId);

            await GetMyHelpRequests(StudentId);
            await GetMyStudyGroups(StudentId);
            await GetMyRides(StudentId);

            Busy = false;
        }

        private async Task GetStudentDetails(int StudentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Students/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var student = JsonConvert.DeserializeObject<Student>(content);
            StudentProfile = student;
        }

        private async Task GetMyHelpRequests(int StudentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/GetByStdId/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Models.HelpRequest>>(content);
            HelpRequestsList = new List<Models.HelpRequest>(list);
        }

        private async Task GetMyStudyGroups(int StudentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ByStudentId/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Models.StudyGroup>>(content);
            StudyGroupsList = new List<Models.StudyGroup>(list);
        }

        private async Task GetMyNotebooks(int StudentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/GetByStudentId/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Ride>>(content);
            RidesList = new List<Ride>(list);
        }

        private async Task GetMyRides(int StudentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Rides/GetStudentRides/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Ride>>(content);
            RidesList = new List<Ride>(list);
        }

    }
}
