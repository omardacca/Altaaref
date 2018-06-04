using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.Notebooks
{
    public class FindNotebookResultsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<ViewNotebookStudent> _resultsList;
        public List<ViewNotebookStudent> ResultsList
        {
            get { return _resultsList; }
            private set
            {
                _resultsList = value;
                OnPropertyChanged(nameof(ResultsList));
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

        private bool _isListEmpty;
        public bool IsListEmpty
        {
            get { return _isListEmpty; }
            set
            {
                SetValue(ref _isListEmpty, value);
            }
        }

        private int? _courseId;
        private string _notebookname;
        private bool _switchStatues;

        public ICommand ItemTappedCommand => new Command<ViewNotebookStudent>(HandleItemTapped);

        public FindNotebookResultsViewModel(IPageService pageService, bool SwitchStatues, int? CourseId=null, string notebookname=null)
        {
            _pageService = pageService;

            _courseId = CourseId;
            _notebookname = notebookname;
            _switchStatues = SwitchStatues;

            var tasks = GetResults();
        }

        private async Task GetResults()
        {
            if(_switchStatues && (_notebookname == null || _notebookname.Trim() == ""))
            {
                await GetByCourseOnly();
            } 
            else if(_switchStatues && (_notebookname != null && _notebookname.Trim() != ""))
            {
                await GetByBoth();
            }
            else
            {
                await GetByNotebookNameOnly();
            }
        }

        private async Task GetByCourseOnly()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Search/ByCourseId/" + _courseId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            ResultsList = new List<ViewNotebookStudent>(list);

            if (ResultsList == null || ResultsList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }

        private async Task GetByBoth()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Search/ByCourseIdAndName/" + _courseId + "/" + _notebookname;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            ResultsList = new List<ViewNotebookStudent>(list);

            if (ResultsList == null || ResultsList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }

        private async Task GetByNotebookNameOnly()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Search/ByNotebookName/" + _notebookname;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            ResultsList = new List<ViewNotebookStudent>(list);

            if (ResultsList == null || ResultsList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }

        private async void HandleItemTapped(ViewNotebookStudent notebookStudent)
        {
            await _pageService.PushAsync(new Views.NotebooksDB.NotebookDetails(notebookStudent));
        }
    }
}
