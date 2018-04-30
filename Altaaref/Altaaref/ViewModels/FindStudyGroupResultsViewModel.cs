using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class FindStudyGroupResultsViewModel : BaseViewModel
    {

//        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<StudyGroupView> _studyGroupList;
        public List<StudyGroupView> StudyGroupList
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

        private StudyGroupView _selectedStudyGroup;
        public StudyGroupView SelectedStudyGroup
        {
            get { return _selectedStudyGroup; }
            set { SetValue(ref _selectedStudyGroup, value); }
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

        private bool _isListEmpty;
        public bool IsListEmpty
        {
            get { return _isListEmpty; }
            set
            {
                SetValue(ref _isListEmpty, value);
            }
        }

        private readonly int _courseId;
        private DateTime fromDate;
        private DateTime toDate;
        private int numOfAttendants;

        private ICommand _viewStudyGroupCommand;
        public ICommand ViewStudyGroupCommand { get { return _viewStudyGroupCommand; } }

        public FindStudyGroupResultsViewModel(IPageService pageService, int courseid, DateTime from, DateTime to, int numOfAttendants)
        {
            _pageService = pageService;
            _viewStudyGroupCommand = new Command<StudyGroupView>(HandleViewStudyGroup);

            this._courseId = courseid;
            this.fromDate = from;
            this.toDate = to;
            this.numOfAttendants = numOfAttendants;

            InitStudyGroupListAsync(numOfAttendants);

        }

        public FindStudyGroupResultsViewModel(IPageService pageService, int courseid, DateTime from, DateTime to)
        {
            _pageService = pageService;
            _viewStudyGroupCommand = new Command<StudyGroupView>(HandleViewStudyGroup);

            this._courseId = courseid;
            this.fromDate = from;
            this.toDate = to;

            InitStudyGroupListAsync(0);
        }

        public void HandleViewStudyGroup(StudyGroupView studyGroupClicked)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(studyGroupClicked));
        }

        private async void InitStudyGroupListAsync(int numOfAttendants)
        {
            Busy = true;

            string url = "";
            if (numOfAttendants == 0)
                url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/" + _courseId + "/" + fromDate.Date.ToString("yyyy-MM-dd") + "/" + toDate.Date.ToString("yyyy-MM-dd");
            else
                url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/" + _courseId + "/" + numOfAttendants + "/" + fromDate.Date.ToString("yyyy-MM-dd") + "/" + toDate.Date.ToString("yyyy-MM-dd");

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroupView>>(results);
            StudyGroupList = new List<StudyGroupView>(list);

            if (StudyGroupList == null || StudyGroupList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }
    }
}
