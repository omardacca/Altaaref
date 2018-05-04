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
    public class NotebookListViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private ObservableCollection<Notebook> _notebookList;
        public ObservableCollection<Notebook> NotebooksList
        {
            get { return _notebookList; }
            set
            {
                _notebookList = value;
                OnPropertyChanged(nameof(NotebooksList));
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
        public NotebookListViewModel(IPageService pageService, int notebookId)
        {
            // Enable activity Indicator - disable is right after assigning listView item source
            Busy = true;

            _pageService = pageService;
            GetNotebooksAsync(notebookId);
        }

        private async void GetNotebooksAsync(int courseId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Course/" + courseId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Notebook>>(content);
            NotebooksList = new ObservableCollection<Notebook>(list);

            // Disable Activity Idicator
            Busy = false;
        }

        private Notebook _selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }

        public async Task NotebookSelectedAsync(Notebook notebook)
        {
            //Deselect Item
            SelectedNotebook = null;

            //await _pageService.PushAsync(new Views.NotebooksDB.NotebookDetails(notebook));
        }


    }
}
