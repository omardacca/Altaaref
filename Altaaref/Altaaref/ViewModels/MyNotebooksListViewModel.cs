﻿using Newtonsoft.Json;
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
        int StudentId = 204228043;
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

        private ICommand _selectedCommand;
        public ICommand SelectedCommand { get { return _selectedCommand; } }

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

            _selectedCommand = new Command<ViewNotebookStudent>(ViewNotebookSelected);
            GetMyNotebooksList();
        }

        public void ViewNotebookSelected(ViewNotebookStudent viewNotebookStudent)
        {
            _pageService.PushAsync(new Views.NotebooksDB.NotebookDetails(viewNotebookStudent));
        }

        public async void GetMyNotebooksList()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/GetStudentNotebooks/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewNotebookStudent>>(content);
            MyNotebooksList = new List<ViewNotebookStudent>(list);

            if (MyNotebooksList == null || MyNotebooksList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }
    }
}