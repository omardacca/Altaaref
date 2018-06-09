using Altaaref.Helpers;
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
    public class Section : BaseViewModel
    {
        public int FacultyId { get; set; }

        public string Text { get; set; }

        private List<CourseInList> _list;
        public List<CourseInList> List
        {
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged(nameof(List));
            }
        }
    }

    public class CourseInList : BaseViewModel
    {
        public int FacultyId { get; set; }
        public Courses Course { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                SetValue(ref _isChecked, value);
            }
        }
    }

    public class ViewModel : BaseViewModel
    {
        private List<Section> _list;
        public List<Section> List
        {
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged(nameof(List));
            }
        }
    }

    public class SelectCoursesForRegisterationViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        public List<Faculty> FacultiesSelectedList;

        private ViewModel _viewModel;
        public ViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;
                OnPropertyChanged(nameof(ViewModel));
            }
        }

        private List<Courses> _coursesSelectedList = new List<Courses>();

        public ICommand ItemTapCommand => new Command<CourseInList>(HandleItemTapped);
        public ICommand DoneCommand => new Command(async () => await PostLists());

        List<List<CourseInList>> ListOfListsOfCourses = new List<List<CourseInList>>();

        private CourseInList _selectedCourse;
        public CourseInList SelectedCourse
        {
            get { return _selectedCourse; }
            set { SetValue(ref _selectedCourse, value); }
        }

        public SelectCoursesForRegisterationViewModel(IPageService pageService, List<Faculty> FacultiesSelectedList)
        {
            _pageService = pageService;
            this.FacultiesSelectedList = FacultiesSelectedList;

            ViewModel = new ViewModel
            {
                List = new List<Section>()
            };
            
            var x = Initial();

        }

        private async Task Initial()
        {
            foreach (var fc in FacultiesSelectedList)
            {
                await GetFacultiesCourses(fc);
                InitSections(fc);
            }


            ViewModel.List = seclist;
        }

        private void HandleItemTapped(CourseInList courses)
        {
            AddOrRemoveCourseFromCoursesList(courses.Course);

            ViewModel.List.Find(sec => sec.FacultyId == courses.FacultyId)
                .List.Find(crslist => crslist.Course.Id == courses.Course.Id).IsChecked = !courses.IsChecked;
        }

        public void AddOrRemoveCourseFromCoursesList(Courses courses)
        {
            var result = _coursesSelectedList.Find(s => s.Id == courses.Id);
            if (result != null)
                _coursesSelectedList.Remove(result);
            else
                _coursesSelectedList.Add(courses);
        }

        List<Section> seclist = new List<Section>();
        private void InitSections(Faculty faculty)
        {
            foreach(var list in ListOfListsOfCourses)
            {
                if (list[0].FacultyId != faculty.Id) continue;
                Section sec = new Section { FacultyId = faculty.Id, Text = faculty.Name, List = list };
                seclist.Add(sec);
            }
            
        }

        private async Task GetFacultiesCourses(Faculty faculty)
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/FacultyCourses/GetCoursesByFacultiesId/" + faculty.Id;

            string content = await _client.GetStringAsync(url);
            var crses = JsonConvert.DeserializeObject<List<Courses>>(content);

            List<CourseInList> crsList = new List<CourseInList>();
            CourseInList crsView;

            foreach (var c in crses)
            {
                crsView = new CourseInList();
                crsView.FacultyId = faculty.Id;
                crsView.Course = c;
                crsView.IsChecked = false;
                crsList.Add(crsView);
            }

            //List<Section> seclist = new List<Section>();
            //Section sec = new Section { FacultyId = faculty.Id, Text = faculty.Name, List = crsList };
            //seclist.Add(sec);

            ListOfListsOfCourses.Add(crsList);

            Busy = false;

        }

        private async Task PostStudentCourses()
        {
            Busy = true;

            var request = new HttpRequestMessage(
                HttpMethod.Post, "https://altaarefapp.azurewebsites.net/api/StudentCourses");

            List<StudentCourses> studentCourses = new List<StudentCourses>();

            foreach(var crs in _coursesSelectedList)
            {
                studentCourses.Add(new StudentCourses
                {
                    CourseId = crs.Id,
                    StudentId = Settings.StudentId
                });
            }

            var serStd = JsonConvert.SerializeObject(studentCourses);

            request.Content = new StringContent(serStd, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Success", "You have been registered successfully", "Ok", "Cancel");

                var page = new Views.MainMenu.MenuPage().GetMenuPage();
                NavigationPage.SetHasNavigationBar(page, false);

                await _pageService.PushAsync(page);
                Busy = false;
            }
            else
            {
                await _pageService.DisplayAlert("Failure", "Something went wrong!", "Ok", "Cancel");
                Busy = false;
            }
        }

        private async Task<bool> PostStudentFaculties()
        {
            Busy = true;

            var request = new HttpRequestMessage(
                HttpMethod.Post, "https://altaarefapp.azurewebsites.net/api/StudentFaculties");

            List<StudentFaculty> studentCourses = new List<StudentFaculty>();

            foreach (var crs in FacultiesSelectedList)
            {
                studentCourses.Add(new StudentFaculty
                {
                    FacultyId = crs.Id,
                    StudentId = Settings.StudentId
                });
            }

            var serStd = JsonConvert.SerializeObject(studentCourses);

            request.Content = new StringContent(serStd, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Busy = false;
                return true;
            }
            else
            {
                Busy = false;
                return false;
            }
        }

        private async Task PostLists()
        {
            if(_coursesSelectedList == null || _coursesSelectedList.Count == 0)
            {
                await _pageService.DisplayAlert("Error", "Please Selected at least one course!", "Ok", "Cancel");
                return;
            }
            await PostStudentFaculties();
            await PostStudentCourses();
        }
    }
}
