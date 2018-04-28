using Altaaref.Models;
using Altaaref.Views.StudyGroups;
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
    public class ViewStudyGroupDetailsViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public StudyGroupView StudyGroupView { get; private set; }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        private List<MiniStudentView> _miniStudentsViewAttendantsList;
        public List<MiniStudentView> MiniStudentsViewAttendantsList
        {
            get
            {
                return _miniStudentsViewAttendantsList;
            }
            private set
            {
                _miniStudentsViewAttendantsList = value;
                OnPropertyChanged(nameof(MiniStudentsViewAttendantsList));
            }
        }

        private List<StudyGroupCommentView> _studyGroupCommentsList;
        public List<StudyGroupCommentView> StudyGroupCommentsList
        {
            get
            {
                return _studyGroupCommentsList;
            }
            private set
            {
                _studyGroupCommentsList = value;
                OnPropertyChanged(nameof(StudyGroupCommentsList));
            }
        }

        public ICommand HandleSeeAllAttendantsCommand { get; private set; }
        public ICommand HandleAttendantButtonCommand { get; private set; }

        private string _attendButtonCaption;
        public string AttendButtonCaption
        {
            get
            {
                if (IsAttendant)
                    AttendButtonCaption = "Leave";
                else
                    AttendButtonCaption = "Join";

                return _attendButtonCaption;
            }
            set
            {
                SetValue(ref _attendButtonCaption, value);
            }
        }

        private bool _isAttendant;
        public bool IsAttendant
        {
            get { return _isAttendant; }
            set
            {
                SetValue(ref _isAttendant, value);
            }
        }

        private bool _commentsEmpty;
        public bool CommentsEmpty
        {
            get { return _commentsEmpty; }
            set
            {
                SetValue(ref _commentsEmpty, value);
            }
        }

        public ViewStudyGroupDetailsViewModel(StudyGroupView studyGroupView, IPageService pageService)
        {
            _pageService = pageService;

            HandleSeeAllAttendantsCommand = new Command(HandleSeeAllAttendants);

            AttendButtonCaption = "Join";
            HandleAttendantButtonCommand = new Command(HandleAttendantButton);

            CommentsEmpty = false;
            StudyGroupView = studyGroupView;

            GetMiniStudentViewAttendants();
            GetComments();

            InitIsAttendantAsync();
        }

        // Ready
        private async void InitIsAttendantAsync()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/" + StudyGroupView.StudyGroupId + "/" + StudentId;

            try
            {
                var content = await _client.GetStringAsync(url);
                var SGA = JsonConvert.DeserializeObject<StudyGroupAttendants>(content);

                if (SGA != null)
                    IsAttendant = true;
            }
            catch(HttpRequestException e)
            {
                IsAttendant = false;
            }
        }

        // Ready
        private async void GetMiniStudentViewAttendants()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/GetMiniStudentView" + StudyGroupView.StudyGroupId;

            string content = await _client.GetStringAsync(url);
            var miniStudentsView = JsonConvert.DeserializeObject<List<MiniStudentView>>(content);
            MiniStudentsViewAttendantsList = miniStudentsView;
        }

        // Ready
        private async void GetComments()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupComments/BySGId/" + StudyGroupView.StudyGroupId;

            try
            {
                var content = await _client.GetStringAsync(url);
                var SGCV = JsonConvert.DeserializeObject<List<StudyGroupCommentView>>(content);
                StudyGroupCommentsList = SGCV;

                // Do I have to keep these two lines ? If 404 then will jump to catch
                // only if not 404 will get into these lines
                if (SGCV != null)
                    CommentsEmpty = false;
            }
            catch (HttpRequestException e)
            {
                CommentsEmpty = true;
            }
        }

        // Ready
        private void HandleSeeAllAttendants()
        {
           _pageService.PushAsync(new ViewAttendants(StudyGroupView.StudyGroupId));
        }

        // Ready
        private void HandleAttendantButton()
        {
            if (IsAttendant)
                HandleRemoveAttendant();
            else
                HandlePostAttendant();
        }

        // Ready
        private void HandlePostAttendant()
        {
            if(StudyGroupView.Date >= DateTime.Now && StudyGroupView.Time > DateTime.Now)
                PostAttendanceAsync();
                IsAttendant = true;
        }

        // Ready
        private void HandleRemoveAttendant()
        {
            if(StudyGroupView.Date <= DateTime.Now && StudyGroupView.Time < DateTime.Now)
                RemoveAttendanceAsync();
                IsAttendant = false;
        }

        // Ready
        private async void PostAttendanceAsync()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants";

            var sga = new StudyGroupAttendants { StudentId = StudentId, StudyGroupId = StudyGroupView.StudyGroupId };

            var content = new StringContent(JsonConvert.SerializeObject(sga), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var StudyGroupInserted = JsonConvert.DeserializeObject<StudyGroupAttendants>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Add Attendant", "You have been added to the Group. ", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with Add Study Group attendant", "OK", "Cancel");
            }
        }

        // Ready
        private async void RemoveAttendanceAsync()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/" + StudyGroupView.StudyGroupId + "/" + StudentId;

            var response = _client.DeleteAsync(postUrl);

            if (!response.Result.IsSuccessStatusCode)
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
        }
    }
}
