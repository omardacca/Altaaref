using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Altaaref.ViewModels
{
    public class FacultyCoursesListViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private ObservableCollection<Courses> _coursesList;
        public ObservableCollection<Courses> CoursesList
        {
            get { return _coursesList; }
            set
            {
                _coursesList = value;
                OnPropertyChanged(nameof(CoursesList));
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

        private readonly IPageService _pageService;
        public FacultyCoursesListViewModel(IPageService pageService, int facultyId)
        {
            // Enable activity Indicator - disable is right after assigning listView item source
            Busy = true;

            _pageService = pageService;
            GetCoursesAsync(facultyId);
        }

        private async void GetCoursesAsync(int facultyId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/FacultyCourses/Courses/" + facultyId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            CoursesList = new ObservableCollection<Courses>(list);

            // Disable Activity Idicator
            Busy = false;
        }

        private Courses _selectedCourse;
        public Courses SelectedCourse
        {
            get { return _selectedCourse; }
            set { SetValue(ref _selectedCourse, value); }
        }

        public async Task CourseSelectedAsync(Courses course)
        {
            //Deselect Item
            SelectedCourse = null;

            await _pageService.PushAsync(new Views.NotebooksDB.NotebooksListPage(course.Id));
        }
    }
}
