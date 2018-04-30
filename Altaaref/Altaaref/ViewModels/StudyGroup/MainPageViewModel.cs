using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.StudyGroup
{
    public class MainPageViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;


        private List<StudyGroupView> _studyGroupsList;
        public List<StudyGroupView> StudyGroupsList
        {
            get { return _studyGroupsList; }
            private set
            {
                _studyGroupsList = value;
                OnPropertyChanged(nameof(StudyGroupsList));
            }
        }

        public ICommand AddButtonCommand => new Command(AddAction);
        public ICommand FindButtonCommand => new Command(FindAction);

        public MainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            GetStudyGroupsByStudentId(StudentId);
        }

        private void AddAction()
        {
            _pageService.PushAsync(new Views.StudyGroups.NewGroupPage());
        }

        private void FindAction()
        {
            _pageService.PushAsync(new Views.StudyGroups.FindStudyGroup());
        }

        private async void GetStudyGroupsByStudentId(int studentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + studentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroupView>>(content);
            StudyGroupsList = list;
        }

        
    }
}
