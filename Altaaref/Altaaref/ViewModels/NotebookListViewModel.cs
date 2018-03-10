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

        private readonly IPageService _pageService;
        public NotebookListViewModel(IPageService pageService, int notebookId)
        {
            _pageService = pageService;
            GetNotebooksAsync(notebookId);
        }

        private async void GetNotebooksAsync(int courseId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/Course/" + courseId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Notebook>>(content);
            NotebooksList = new ObservableCollection<Notebook>(list);
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

            await _pageService.PushAsync(new Views.NotebooksDB.NotebooksListPage(notebook.Id));
        }


    }
}
