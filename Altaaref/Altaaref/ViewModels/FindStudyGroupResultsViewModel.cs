using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class FindStudyGroupResultsViewModel : BaseViewModel
    {

//        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<Models.StudyGroup> _studyGroupList;
        public List<Models.StudyGroup> StudyGroupList
        {
            get
            {
                return _studyGroupList;
            }
            private set
            {
                _studyGroupList = value;
                OnPropertyChanged(nameof(StudyGroupList));
            }
        }

        private Models.StudyGroup _selectedStudyGroup;
        public Models.StudyGroup SelectedStudyGroup
        {
            get { return _selectedStudyGroup; }
            set { SetValue(ref _selectedStudyGroup, value); }
        }

        private readonly int _courseId;
        private DateTime fromDate;
        private DateTime toDate;
        private int numOfAttendants;


        public FindStudyGroupResultsViewModel(IPageService pageService, int courseid, DateTime from, DateTime to, int numOfAttendants)
        {
            _pageService = pageService;

            this._courseId = courseid;
            this.fromDate = from;
            this.toDate = to;
            this.numOfAttendants = numOfAttendants;

            InitStudyGroupListAsync(numOfAttendants);
        }

        public FindStudyGroupResultsViewModel(IPageService pageService, int courseid, DateTime from, DateTime to)
        {
            _pageService = pageService;

            this._courseId = courseid;
            this.fromDate = from;
            this.toDate = to;

            InitStudyGroupListAsync(0);

        }

        public void StudyGroupResultItemClicked(Models.StudyGroup studyGroupClicked)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(studyGroupClicked));
        }

        private async void InitStudyGroupListAsync(int numOfAttendants)
        {
            string url = "";
            if (numOfAttendants == 0)
                url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/" + _courseId + "/" + fromDate.Date.ToString("yyyy-MM-dd") + "/" + toDate.Date.ToString("yyyy-MM-dd");
            else
                url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/" + _courseId + "/" + numOfAttendants + "/" + fromDate.Date.ToString("yyyy-MM-dd") + "/" + toDate.Date.ToString("yyyy-MM-dd");

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Models.StudyGroup>>(results);
            StudyGroupList = new List<Models.StudyGroup>(list);
        }
    }
}
