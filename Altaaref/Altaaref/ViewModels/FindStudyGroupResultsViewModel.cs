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

        private StudyGroup _studyGroup;
        public StudyGroup StudyGroup
        {
            get { return _studyGroup; }
            private set { SetValue(ref _studyGroup, value); }
        }

        private List<StudyGroup> _studyGroupList;
        public List<StudyGroup> StudyGroupList
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

        public FindStudyGroupResultsViewModel(StudyGroup studyGroup, IPageService pageService)
        {
            _pageService = pageService;

            StudyGroup = studyGroup;
            InitAsync();
        }

        private async void InitAsync()
        {
            await InitStudyGroupListAsync();
        }

        private async System.Threading.Tasks.Task InitStudyGroupListAsync()
        {
            StudyGroup.Date = new DateTime(1, 1, 1);
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ByDate";
            //var content = new StringContent(JsonConvert.SerializeObject(StudyGroup), Encoding.UTF8, "application/json");

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroup>>(results);
            StudyGroupList = new List<StudyGroup>(list);
        }
    }
}
