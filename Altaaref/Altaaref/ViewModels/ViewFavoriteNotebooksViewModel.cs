using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewNotebookStudent
    {
        public Notebook Notebook { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }

    public class ViewFavoriteNotebooksViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<ViewNotebookStudent> _viewFavNotebooksList;
        public List<ViewNotebookStudent> ViewFavNotebooksList
        {
            get { return _viewFavNotebooksList; }
            private set
            {
                _viewFavNotebooksList = value;
                OnPropertyChanged(nameof(ViewFavNotebooksList));
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

        public ViewFavoriteNotebooksViewModel(IPageService pageService)
        {
            _pageService = pageService;
            GetFavoriteNotebooksList();
        }

        private async void GetFavoriteNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/Details/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            ViewFavNotebooksList = new List<ViewNotebookStudent>(list);
            Busy = false;
        }

    }
}
