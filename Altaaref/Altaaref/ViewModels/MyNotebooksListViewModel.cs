using Altaaref.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class MyNotebooksListViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;


        private List<ViewNotebookStudent> _myNotebooksList;
        public List<ViewNotebookStudent> MyNotebooksList
        {
            get { return _myNotebooksList; }
            private set
            {
                _myNotebooksList = value;
                OnPropertyChanged(nameof(MyNotebooksList));
            }
        }

        public ICommand ItemTappedCommand => new Command<ViewNotebookStudent>(ViewNotebookSelected);

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



        public MyNotebooksListViewModel(IPageService pageService)
        {
            _pageService = pageService;

            GetMyNotebooksList();
        }

        private async void ViewNotebookSelected(ViewNotebookStudent viewNotebookStudent)
        {
            await _pageService.PushAsync(new Views.NotebooksDB.NotebookDetails(viewNotebookStudent));
        }

        public async void GetMyNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/GetStudentNotebooks/" + Settings.Identity;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            MyNotebooksList = new List<ViewNotebookStudent>(list);

            if (MyNotebooksList == null || MyNotebooksList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }
    }
}
