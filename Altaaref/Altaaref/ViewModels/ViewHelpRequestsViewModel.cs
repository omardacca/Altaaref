using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewHelpRequestsViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private bool IsGeneral = false;

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

        private StudentHelpRequest _selectedStudentHelpRequest;
        public StudentHelpRequest SelectedStudentHelpRequest
        {
            get { return _selectedStudentHelpRequest; }
            set { SetValue(ref _selectedStudentHelpRequest, value); }
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
        
        public ViewHelpRequestsViewModel(IPageService pageService, bool IsGeneral = false)
        {
            _pageService = pageService;

            this.IsGeneral = IsGeneral;
            if(IsGeneral)
                InitGeneralHelpRequests();
        }

        public async void HandleItemClicked(StudentHelpRequest studentHelpRequest)
        {
            await _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }

        private async void InitGeneralHelpRequests()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/General";
            
            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);

            Busy = false;
        }
    }
}
