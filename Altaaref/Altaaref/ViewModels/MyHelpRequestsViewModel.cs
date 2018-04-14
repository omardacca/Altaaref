using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{

    public class StudentsHelpRequests : BaseViewModel
    {
        private HelpRequest _helpRequestsList;
        public HelpRequest HelpRequestsList
        {
            get
            {
                return _helpRequestsList;
            }
            private set
            {
                _helpRequestsList = value;
                OnPropertyChanged(nameof(HelpRequestsList));
            }
        }

        private Student _student;
        public Student Student
        {
            get { return _student; }
            set { SetValue(ref _student, value); }
        }
    }

    public class MyHelpRequestsViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        Student Student;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<StudentsHelpRequests> _studentHelpRequest;
        public List<StudentsHelpRequests> StudentHelpRequest
        {
            get
            {
                return _studentHelpRequest;
            }
            private set
            {
                _studentHelpRequest = value;
                OnPropertyChanged(nameof(StudentHelpRequest));
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

        public MyHelpRequestsViewModel(IPageService pageService)
        {
            _pageService = pageService;
            GetStudentDetails();
            GetMyHelpRequests();
            UpdateStudentInStudentHelpRequests();
        }

        private void UpdateStudentInStudentHelpRequests()
        {
            foreach (var sh in StudentHelpRequest)
                sh.Student = Student;
        }

        private async void GetMyHelpRequests()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/ByStudentId/" + StudentId;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentsHelpRequests>>(results);
            StudentHelpRequest = new List<StudentsHelpRequests>(list);

            Busy = false;
        }

        private async void GetStudentDetails()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/Students/" + StudentId;

            string results = await _client.GetStringAsync(url);
            var student = JsonConvert.DeserializeObject<Student>(results);
            Student = student;

            Busy = false;
        }
    }
}
