using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels.StudyGroup
{
    public class MainPageViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;


        private List<StudyGroupView> _studyGroupsList;
        public List<StudyGroupView> StudyGroupsList
        {
            get { return _studyGroupsList; }
            private set
            {
                _studyGroupsList = value;
                OnPropertyChanged(nameof(StudyGroupsList));
            }
        }


        public MainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            GetStudyGroupsByStudentId(StudentId);
        }

        private async void GetStudyGroupsByStudentId(int studentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + studentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroupView>>(content);
            StudyGroupsList = list;
        }

    }
}
