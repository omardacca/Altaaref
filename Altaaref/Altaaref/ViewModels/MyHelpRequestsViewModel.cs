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
    public class StudentHelpRequest : BaseViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsGeneral { get; set; }

        private bool _isMet;
        public bool IsMet
        {
            get { return _isMet; }
            set { SetValue(ref _isMet, value); }
        }
        public int Views { get; set; }
        public DateTime Date { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

    }

    public class MyHelpRequestsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;
        

        private List<StudentHelpRequest> _helpRequestsList;
        public List<StudentHelpRequest> HelpRequestsList
        {
            get { return _helpRequestsList; }
            private set
            {
                _helpRequestsList = value;
                OnPropertyChanged(nameof(HelpRequestsList));
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

        private ICommand _viewCommand;
        public ICommand ViewCommand { get { return _viewCommand; } }

        private StudentHelpRequest _selectedStudentHelpRequest;
        public StudentHelpRequest SelectedStudentHelpRequest
        {
            get { return _selectedStudentHelpRequest; }
            set { SetValue(ref _selectedStudentHelpRequest, value); }
        }

        public MyHelpRequestsViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _viewCommand = new Command<StudentHelpRequest>(HandleViewCommand);

            InitHelpRequests();
        }

        private void HandleViewCommand(StudentHelpRequest studentHelpRequest)
        {
            _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }

        private async void InitHelpRequests()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/ByStudentId/" + Settings.Identity;
            
            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);

            if (HelpRequestsList == null || HelpRequestsList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }

    }
}
