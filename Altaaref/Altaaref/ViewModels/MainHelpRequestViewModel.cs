using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class MainHelpRequestViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<StudentHelpRequest> _generalHelpRequestsList;
        public List<StudentHelpRequest> GeneralHelpRequestsList
        {
            get { return _generalHelpRequestsList; }
            private set
            {
                _generalHelpRequestsList = value;
                OnPropertyChanged(nameof(GeneralHelpRequestsList));
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


        public MainHelpRequestViewModel(IPageService pageService)
        {
            _pageService = pageService;

            GetGeneralHelpRequest();
        }

        public async void GetGeneralHelpRequest()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/General";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            GeneralHelpRequestsList = new List<StudentHelpRequest>(list);
        }

        public async void HandleResultClicked(StudentHelpRequest studentHelpRequest)
        {
            await _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }


    }
}
