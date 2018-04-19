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

        private Models.StudyGroup _studyGroup;
        public Models.StudyGroup StudyGroup
        {
            get { return _studyGroup; }
            private set { SetValue(ref _studyGroup, value); }
        }

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


        public FindStudyGroupResultsViewModel(Models.StudyGroup studyGroup, IPageService pageService)
        {
            _pageService = pageService;

            StudyGroup = studyGroup;
            InitAsync();
        }

        public void StudyGroupResultItemClicked(Models.StudyGroup studyGroupClicked)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(studyGroupClicked));
        }

        private async void InitAsync()
        {
            await InitStudyGroupListAsync();
        }

        private async System.Threading.Tasks.Task InitStudyGroupListAsync()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/" + StudyGroup.CourseId + "/" + StudyGroup.Date.Date;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Models.StudyGroup>>(results);
            StudyGroupList = new List<Models.StudyGroup>(list);
        }
    }
}
