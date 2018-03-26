using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class NewGroupViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private StudyGroup _studyGroup;
        public StudyGroup StudyGroup
        {
            get { return _studyGroup; }
            private set { SetValue(ref _studyGroup, value); }
        }

        public ICommand HandleSubmition { get; private set; }

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

        public NewGroupViewModel(IPageService pageService)
        {
            _pageService = pageService;

            Init();
        }

        async void Init()
        {
            Busy = true;
            StudyGroup = new StudyGroup();
            CoursesNameList = new List<string>();
            HandleSubmition = new Command(OnSubmitButtonTapped);
            await GetCoursesAsync();
            InitCoursesList();
            Busy = false;
        }

        private void InitCoursesList()
        {
            foreach (var course in CoursesList)
                CoursesNameList.Add(course.Name);
        }


        // SHOULD BE: STUDENT COURSES not ALL COURSES
        private async Task GetCoursesAsync()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Courses";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            CoursesList = new List<Courses>(list);
        }

        private async void OnSubmitButtonTapped()
        {

            if (!isFormValid())
            {
                await _pageService.DisplayAlert("Error", "Please fill the form properly.", "OK", "Cancel");
                return;
            }
                

            var courseid = _coursesList[_selectedCourseIndex].Id;

            StudyGroup.CourseId = courseid;
            StudyGroup.StudentId = StudentId;


            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroups";

            var content = new StringContent(JsonConvert.SerializeObject(StudyGroup), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Group Created", "The group created successfuly", "OK", "Cancel");
                
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
            }
        }

        private bool isFormValid()
        {
            if (StudyGroup.Title != null && StudyGroup.Message != null && StudyGroup.Time != null && StudyGroup.Date != null)
                return true;
            return false;
        }
    }
}
