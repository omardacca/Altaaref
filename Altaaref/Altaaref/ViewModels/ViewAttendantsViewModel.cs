using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewAttendantsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<ViewStudent> _studentList;
        public List<ViewStudent> StudentsList
        {
            get { return _studentList; }
            private set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentsList));
            }
        }

        private int StudyGroupId;

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        public ViewAttendantsViewModel(IPageService pageService, int StudyGroupId)
        {
            _pageService = pageService;
            this.StudyGroupId = StudyGroupId;
            Init();
        }

        public void Init()
        {
            InitStudentList();
        }

        private async void InitStudentList()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/GetNames/" + StudyGroupId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Student>>(content);
            var stdView = new List<ViewStudent>();
            foreach (Student std in list)
                stdView.Add(new ViewStudent { Student = std });

            StudentsList = new List<ViewStudent>(stdView);
            Busy = false;
        }

    }
}
