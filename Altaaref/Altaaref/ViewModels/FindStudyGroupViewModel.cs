using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class FindStudyGroupViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private Models.StudyGroup _studyGroup;
        public Models.StudyGroup StudyGroup
        {
            get { return _studyGroup; }
            private set { SetValue(ref _studyGroup, value); }
        }

        private List<Courses> _coursesList;
        public List<Courses> CoursesList
        {
            get
            {
                return _coursesList;
            }
            private set
            {
                _coursesList = value;
                OnPropertyChanged(nameof(CoursesList));
            }
        }

        private List<string> _coursesNamesList;
        public List<string> CoursesNameList
        {
            get { return _coursesNamesList; }
            set
            {
                _coursesNamesList = value;
                OnPropertyChanged(nameof(CoursesNameList));
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

        private int _selectedCourseIndex;
        public int SelectedCourseIndex
        {
            get { return _selectedCourseIndex; }
            set { SetValue(ref _selectedCourseIndex, value); }
        }

        public ICommand HandleSubmitFind { get; private set; }

        public FindStudyGroupViewModel(IPageService pageService)
        {
            _pageService = pageService;
            InitAsync();
        }

        async void InitAsync()
        {
            StudyGroup = new Models.StudyGroup() { Date = DateTime.Today, Time = DateTime.Now.AddHours(5) };
            CoursesNameList = new List<string>();
            HandleSubmitFind = new Command(OnHandleFindSubmitButtonTapped);
            await GetCoursesAsync();
        }

        private void OnHandleFindSubmitButtonTapped(object obj)
        {
            var courseid = _coursesList[_selectedCourseIndex].Id;

            StudyGroup.CourseId = courseid;
            StudyGroup.StudentId = StudentId;

            _pageService.PushAsync(new Views.StudyGroups.FindStudyGroupResults(StudyGroup));
        }

        // SHOULD BE: STUDENT COURSES not ALL COURSES
        private async Task GetCoursesAsync()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/Courses";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            CoursesList = new List<Courses>(list);

            Busy = false;
        }

        private bool IsFormValid()
        {
            if (StudyGroup.Date != null)
                return true;
            return false;
        }
    }
}
