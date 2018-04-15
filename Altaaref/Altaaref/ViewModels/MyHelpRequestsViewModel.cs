using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class StudentHelpRequest : BaseViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsGeneral { get; set; }

        private bool _isMet;
        public bool IsMet
        {
            get { return _isMet; }
            set { SetValue(ref _isMet, value); }
        }
        public int Views { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

    }

    public class MyHelpRequestsViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;
        

        private List<StudentHelpRequest> _helpRequestsList;
        public List<StudentHelpRequest> HelpRequestsList
        {
            get { return _helpRequestsList; }
            private set
            {
                _helpRequestsList = value;
                OnPropertyChanged(nameof(HelpRequestsList));
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

        private StudentHelpRequest _selectedStudentHelpRequest;
        public StudentHelpRequest SelectedStudentHelpRequest
        {
            get { return _selectedStudentHelpRequest; }
            set { SetValue(ref _selectedStudentHelpRequest, value); }
        }

        public void HandleItemClicked(StudentHelpRequest studentHelpRequest)
        {
            //Deselect Item
            SelectedStudentHelpRequest = null;

            _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }

        public MyHelpRequestsViewModel(IPageService pageService)
        {
            _pageService = pageService;

            InitHelpRequests();
        }

        private async void InitHelpRequests()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/ByStudentId/" + StudentId;
            
            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);
            
            Busy = false;
        }

    }
}
