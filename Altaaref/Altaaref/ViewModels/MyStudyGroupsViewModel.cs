using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class MyStudyGroupsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<StudyGroupView> _studyGroupList;
        public List<StudyGroupView> StudyGroupList
        {
            get
            {
                return _studyGroupList;
            }
            private set
            {
                _studyGroupList = value;
                OnPropertyChanged(nameof(StudyGroupList));
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

        private StudyGroupView _selectedStudyGroup;
        public StudyGroupView SelectedStudyGroup
        {
            get { return _selectedStudyGroup; }
            set { SetValue(ref _selectedStudyGroup, value); }
        }

        private ICommand _viewStudyGroupCommand;
        public ICommand ViewStudyGroupCommand { get { return _viewStudyGroupCommand; } }

        public MyStudyGroupsViewModel(IPageService pageService)
        {
            _pageService = pageService;

            _viewStudyGroupCommand = new Command<StudyGroupView>(StudyGroupItemClicked);

            InitAsync();
        }

        private async void InitAsync()
        {
            await InitStudyGroupListAsync();
        }

        public void StudyGroupItemClicked(StudyGroupView studyGroupClicked)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(studyGroupClicked));
        }

        private async Task InitStudyGroupListAsync()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + Settings.Identity;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroupView>>(results);
            StudyGroupList = new List<StudyGroupView>(list);

            if (StudyGroupList == null || StudyGroupList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }
    }
}
