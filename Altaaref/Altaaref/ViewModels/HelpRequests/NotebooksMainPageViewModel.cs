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

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        private List<ViewNotebookStudent> _recentNotebooksList;
        public List<ViewNotebookStudent> RecentNotebooksList
        {
            get { return _recentNotebooksList; }
            private set
            {
                _recentNotebooksList = value;
                OnPropertyChanged(nameof(RecentNotebooksList));
            }
        }

        private List<ViewNotebookStudent> _topRatedNotebooksList;
        public List<ViewNotebookStudent> TopRatedNotebooksList
        {
            get { return _topRatedNotebooksList; }
            private set
            {
                _topRatedNotebooksList = value;
                OnPropertyChanged(nameof(TopRatedNotebooksList));
            }
        }

        public ICommand AddButtonCommand => new Command(AddAction);
        public ICommand FindButtonCommand => new Command(FindAction);

        public NotebooksMainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            var tasks = InitLists();
        }

        private async Task InitLists()
        {
            await GetRecentNotebooksList();
            await GetTopRatedNotebooksList();
        }

        private async Task GetRecentNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Recent/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            RecentNotebooksList = new List<ViewNotebookStudent>(list);

            Busy = false;
        }

        private async Task GetTopRatedNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/TopRated/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            TopRatedNotebooksList = new List<ViewNotebookStudent>(list);

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
