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
    public class FindNotebookViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<Courses> _coursesList;
        public List<Courses> CoursesList
        {
            get { return _coursesList; }
            private set
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

        private string _notebooknameText;
        public string NotebookNameText
        {
            get { return _notebooknameText; }
            set { SetValue(ref _notebooknameText, value); }
        }

        private bool _coursesSwitch;
        public bool CoursesSwitch
        {
            get { return _coursesSwitch; }
            set
            {
                SetValue(ref _coursesSwitch, value);
            }
        }

        private int _selectedCourseIndex;
        public int SelectedCourseIndex
        {
            get { return _selectedCourseIndex; }
            set { SetValue(ref _selectedCourseIndex, value); }
        }

        public ICommand SearchCommand => new Command(HandleSearchCommand);

        public FindNotebookViewModel(IPageService pageService)
        {
            _pageService = pageService;
            CoursesSwitch = false;

            var tasks = InitObjects();
        }

        private async Task InitObjects()
        {
            await GetStudentCourses();
        }

        private async Task GetStudentCourses()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Courses/GetStudentCourses/" + Settings.Identity;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            CoursesList = new List<Courses>(list);
        }

        private async void HandleSearchCommand ()
        {
            if (!IsFormValid()) return;

            if (_coursesSwitch && (_notebooknameText == null || _notebooknameText.Trim() == ""))
            {
                await _pageService.PushAsync(new Views.NotebooksDB.FindNotebookResults(_coursesSwitch, CoursesList[_selectedCourseIndex].Id));
            }
            else if (_coursesSwitch && (_notebooknameText != null && _notebooknameText.Trim() != ""))
            {
                await _pageService.PushAsync(new Views.NotebooksDB.FindNotebookResults(_coursesSwitch, CoursesList[_selectedCourseIndex].Id, _notebooknameText));
            }
            else
            {
                await _pageService.PushAsync(new Views.NotebooksDB.FindNotebookResults(_coursesSwitch, null, _notebooknameText));
            }

        }

        private bool IsFormValid()
        {
            if ((_notebooknameText == null || _notebooknameText.Trim() == "") && _selectedCourseIndex < 0)
                return false;
            return true;
        }

    }
}
