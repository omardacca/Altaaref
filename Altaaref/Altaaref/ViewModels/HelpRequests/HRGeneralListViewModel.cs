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
    public class HRGeneralListViewModel : BaseViewModel
    {
        int StudentId = 204228043;

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

        public HRGeneralListViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _viewCommand = new Command<StudentHelpRequest>(HandleViewCommand);

            var task = InitHelpRequests();
        }

        private void HandleViewCommand(StudentHelpRequest studentHelpRequest)
        {
            _pageService.PushAsync(new Views.CommonPages.ViewHelpRequestsDetails(studentHelpRequest));
        }

        private async Task InitHelpRequests()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/General";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudentHelpRequest>>(content);

            HelpRequestsList = new List<StudentHelpRequest>(list);

            if (HelpRequestsList == null || HelpRequestsList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }

    }
}
