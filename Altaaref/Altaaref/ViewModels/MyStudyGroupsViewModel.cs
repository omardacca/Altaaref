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
    public class MyStudyGroupsViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

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

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        private int _selectedStudyGroupId;
        public int SelectedStudyGroupId
        {
            get { return _selectedStudyGroupId; }
            set { SetValue(ref _selectedStudyGroupId, value); }
        }

        public MyStudyGroupsViewModel(IPageService pageService)
        {
            _pageService = pageService;
            InitAsync();
        }

        private async void InitAsync()
        {
            await InitStudyGroupListAsync();
        }

        public void StudyGroupItemClicked(StudyGroup studyGroupClicked)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(studyGroupClicked));
        }

        private async System.Threading.Tasks.Task InitStudyGroupListAsync()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + StudentId;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroup>>(results);
            StudyGroupList = new List<StudyGroup>(list);

            Busy = false;
        }
    }
}
