using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Altaaref.Models;
using Newtonsoft.Json;

namespace Altaaref.ViewModels
{
    public class NotebookDetailsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private Notebook _notebook;
        public Notebook Notebook
        {
            get { return _notebook; }
            set
            {
                _notebook = value;
                OnPropertyChanged(nameof(Notebook));
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

        public NotebookDetailsViewModel(int notebookId)
        {
            // Enable activity Indicator - disable is right after assigning listView item source
            Busy = true;

            GetNotebookAsync(notebookId);
        }

        private async void GetNotebookAsync(int notebookId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/" + notebookId;

            string content = await _client.GetStringAsync(url);
            Notebook = JsonConvert.DeserializeObject<Notebook>(content);

            // Disable Activity Idicator
            Busy = false;
        }
    }
}
