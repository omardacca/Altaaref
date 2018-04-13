using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class MyHelpRequestsViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<HelpRequest> _helpRequestsList;
        public List<HelpRequest> HelpRequestsList
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
        }

        private async void GetMyHelpRequests()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + StudentId;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<HelpRequest>>(results);
            HelpRequestsList = new List<HelpRequest>(list);

            Busy = false;
        }
    }
}
