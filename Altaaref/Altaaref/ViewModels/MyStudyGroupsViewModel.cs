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

        public void StudyGroupItemClicked(Models.StudyGroup studyGroupClicked)
        {
            // lookup this page!! the convertion to StudyGroupView in here is temp
            StudyGroupView temp = new StudyGroupView
            {
                StudyGroupId = studyGroupClicked.Id,
                CourseId = studyGroupClicked.CourseId,
                CourseName = "This Imaplementation is temp",
                StudentName = "This Imaplementation is temp",
                Address = studyGroupClicked.Address,
                Date = studyGroupClicked.Date,
                Message = studyGroupClicked.Message,
                Time = studyGroupClicked.Time,
                NumberOfAttendants = 100
            };

            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(temp));
        }

        private async System.Threading.Tasks.Task InitStudyGroupListAsync()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + StudentId;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Models.StudyGroup>>(results);
            StudyGroupList = new List<Models.StudyGroup>(list);

            Busy = false;
        }
    }
}
