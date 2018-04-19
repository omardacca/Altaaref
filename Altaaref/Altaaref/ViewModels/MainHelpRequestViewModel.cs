using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class MainHelpRequestViewModel : BaseViewModel
    {
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

        private bool _busyGeneralHelpRequest;
        public bool BusyGeneralHelpRequest
        {
            get { return _busyGeneralHelpRequest; }
            set
            {
                SetValue(ref _busyGeneralHelpRequest, value);
            }
        }

        private StudentHelpRequest _selectedGeneralHelpRequest;
        public StudentHelpRequest SelectedGeneralHelpRequest
        {
            get { return _selectedGeneralHelpRequest; }
            set { SetValue(ref _selectedGeneralHelpRequest, value); }
        }
                
        private DateTime restrictDate;
        private int facultyId;

        #region ctors

        public MainHelpRequestViewModel(IPageService pageService)
        {
            _pageService = pageService;

            GetGeneralHelpRequest();
        }

        public MainHelpRequestViewModel(IPageService pageService, DateTime restrictDate)
        {
            _pageService = pageService;

            this.restrictDate = restrictDate;

            GetNotGeneralHelpRequest();
        }

        public MainHelpRequestViewModel(IPageService pageService, DateTime restrictDate, int facultyId)
        {
            _pageService = pageService;

            this.restrictDate = restrictDate;
            this.facultyId = facultyId;

            GetNotGeneralWithFacultyIdHelpRequest();
        }

        #endregion

        #region GET REQUESTS

        public async void GetGeneralHelpRequest()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/General";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);
        }

        public async void GetNotGeneralHelpRequest()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/AllNotGeneral/" + this.restrictDate.Date.Date;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);
        }

        public async void GetNotGeneralWithFacultyIdHelpRequest()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpFaculties/" + facultyId + "/" + restrictDate.Date.Date;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);
        }

        #endregion

        public async void HandleResultClicked(StudentHelpRequest studentHelpRequest)
        {
            await _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }


    }
}
