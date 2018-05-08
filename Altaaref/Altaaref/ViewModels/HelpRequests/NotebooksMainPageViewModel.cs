using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.HelpRequests
{
    public class NotebooksMainPageViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private class Rates
        {
            public int NotebookId { get; set; }
            public int Sum { get; set; }
        }

        private List<Courses> _freeNotebookCourses;

        private Courses _freeCourse;
        public Courses FreeCourse
        {
            get { return _freeCourse; }
            set
            {
                _freeCourse = value;
                OnPropertyChanged(nameof(FreeCourse));
            }
        }

        
        private bool _isFreeNotebooksEmpty;
        public bool IsFreeNotebooksEmpty
        {
            get { return _isFreeNotebooksEmpty; }
            set
            {
                SetValue(ref _isFreeNotebooksEmpty, value);
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

        private bool _isRecentListEmpty;
        public bool IsRecentListEmpty
        {
            get { return _isRecentListEmpty; }
            set
            {
                SetValue(ref _isRecentListEmpty, value);
            }
        }

        private bool _isTopRatedListEmpty;
        public bool IsTopRatedListEmpty
        {
            get { return _isTopRatedListEmpty; }
            set
            {
                SetValue(ref _isTopRatedListEmpty, value);
            }
        }

        private List<ViewNotebookStudent> _recentNotebooksList;
        public List<ViewNotebookStudent> RecentNotebooksList
        {
            get { return _recentNotebooksList; }
            set
            {
                _recentNotebooksList = value;
                OnPropertyChanged(nameof(RecentNotebooksList));
            }
        }

        private List<ViewNotebookStudent> _topRatedNotebooksList;
        public List<ViewNotebookStudent> TopRatedNotebooksList
        {
            get { return _topRatedNotebooksList; }
            set
            {
                _topRatedNotebooksList = value;
                OnPropertyChanged(nameof(TopRatedNotebooksList));
            }
        }

        public ICommand AddButtonCommand => new Command(AddAction);
        public ICommand FindButtonCommand => new Command(FindAction);
        public ICommand ItemTappedCommand => new Command<ViewNotebookStudent>(HandleItemTapped);

        public NotebooksMainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            var tasks = InitLists();

            if(!_isFreeNotebooksEmpty)
            {
                Random rnd = new Random();
                int selected = rnd.Next(_freeNotebookCourses.Count);

                FreeCourse = _freeNotebookCourses[selected];
            }

        }

        private async Task InitLists()
        {
            await GetRecentNotebooksList();
            await GetTopRatedNotebooksList();
            await GetEmptyNotebooksCoursesList();
        }

        private async void HandleItemTapped(ViewNotebookStudent vns)
        {
            await _pageService.PushAsync(new Views.NotebooksDB.NotebookDetails(vns));
        }

        private async Task GetRecentNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Recent/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            RecentNotebooksList = new List<ViewNotebookStudent>(list);

            if (RecentNotebooksList == null || RecentNotebooksList.Count == 0)
                IsRecentListEmpty = true;

            Busy = false;
        }

        private async Task GetTopRatedNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/TopRated/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            TopRatedNotebooksList = new List<ViewNotebookStudent>(list);

            if (TopRatedNotebooksList == null || TopRatedNotebooksList.Count == 0)
                IsTopRatedListEmpty = true;

            Busy = false;
        }

        private async Task GetEmptyNotebooksCoursesList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/StudentCourses/GetFreeNotebookCourses/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            _freeNotebookCourses = new List<Courses>(list);

            if (_freeNotebookCourses == null || _freeNotebookCourses.Count == 0)
                IsFreeNotebooksEmpty = true;

            Busy = false;
        }



        private void AddAction()
        {
            //_pageService.PushAsync(new Views.StudyGroups.NewGroupPage());
        }

        private void FindAction()
        {
            //_pageService.PushAsync(new Views.StudyGroups.FindStudyGroup());
        }
    }
}
