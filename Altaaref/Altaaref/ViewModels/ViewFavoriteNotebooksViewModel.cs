using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

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


        public ICommand ItemTappedCommand => new Command<ViewNotebookStudent>(ViewFavoriteNotebookSelected);

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

        public ViewFavoriteNotebooksViewModel(IPageService pageService)
        {
            _pageService = pageService;

            GetFavoriteNotebooksList();
        }

        public void ViewFavoriteNotebookSelected(ViewNotebookStudent viewNotebookStudent)
        {
            _pageService.PushAsync(new Views.NotebooksDB.NotebookDetails(viewNotebookStudent));
        }

        public async void GetFavoriteNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/Details/" + Settings.Identity;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            ViewFavNotebooksList = new List<ViewNotebookStudent>(list);

            if (ViewFavNotebooksList == null || ViewFavNotebooksList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }

    }
}
