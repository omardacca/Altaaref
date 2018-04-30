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

        private StudyGroupView _studyGroupView;
        public StudyGroupView StudyGroupView
        {
            get { return _studyGroupView; }
            set
            {
                _studyGroupView = value;
                OnPropertyChanged(nameof(StudyGroupView));
            }
        }

        private StudyGroupComment _newComment;
        public StudyGroupComment NewComment
        {
            get { return _newComment; }
            set
            {
                _newComment = value;
                OnPropertyChanged(nameof(NewComment));
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
        public ICommand HandleAddCommandButtonCommand { get; private set; }

        private string _attendButtonCaption;
        public string AttendButtonCaption
        {
            get
            {
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

        private bool _attendantslistempty;
        public bool AttendantsListEmpty
        {
            get { return _attendantslistempty; }
            set
            {
                SetValue(ref _attendantslistempty, value);
            }
        }

        private bool _attendButtonPlaying;
        public bool AttendButtonPlaying
        {
            get { return _attendButtonPlaying; }
            set
            {
                SetValue(ref _attendButtonPlaying, value);
            }
        }

        public ViewStudyGroupDetailsViewModel(StudyGroupView studyGroupView, IPageService pageService)
        {
            _pageService = pageService;

            
            HandleAddCommandButtonCommand = new Command(HandlePostNewCommand);
            NewComment = new StudyGroupComment();
            

            HandleSeeAllAttendantsCommand = new Command(HandleSeeAllAttendants);
            AttendButtonCaption = "Join";

            CommentsEmpty = false;
            StudyGroupView = studyGroupView;
            GetMiniStudentViewAttendants();
            GetComments();

            InitIsAttendantAsync();
            HandleAttendantButtonCommand = new Command(HandleAttendantButton);

        }

        // Ready
        private async void InitIsAttendantAsync()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/" + StudyGroupView.StudyGroupId + "/" + StudentId;

            try
            {
                var content = await _client.GetStringAsync(url);
                var SGA = JsonConvert.DeserializeObject<StudyGroupAttendants>(content);

                if (SGA != null)
                {
                    IsAttendant = true;
                    AttendButtonCaption = "Leave";
                }
            }
            catch(HttpRequestException e)
            {
                IsAttendant = false;
                AttendButtonCaption = "Join";
                Busy = false;
            }
            Busy = false;
        }

        // Ready
        private async void GetMiniStudentViewAttendants()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/GetMiniStudentView/" + StudyGroupView.StudyGroupId;

            try
            {
                string content = await _client.GetStringAsync(url);
                var miniStudentsView = JsonConvert.DeserializeObject<List<MiniStudentView>>(content);
                MiniStudentsViewAttendantsList = miniStudentsView;

                if (MiniStudentsViewAttendantsList != null && MiniStudentsViewAttendantsList.Count != 0)
                    AttendantsListEmpty = false;
                else
                    AttendantsListEmpty = true;
            }
            catch(HttpRequestException e)
            {
                AttendantsListEmpty = true;
            }

            Busy = false;
        }

        // Ready
        private async void GetComments()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroupComments/BySGId/" + StudyGroupView.StudyGroupId;

            try
            {
                var content = await _client.GetStringAsync(url);
                var SGCV = JsonConvert.DeserializeObject<List<StudyGroupCommentView>>(content);
                StudyGroupCommentsList = SGCV;

                // Do I have to keep these two lines ? If 404 then will jump to catch
                // only if not 404 will get into these lines
                if (SGCV != null && SGCV.Count != 0)
                    CommentsEmpty = false;
                else
                    CommentsEmpty = true;
            }
            catch (HttpRequestException e)
            {
                CommentsEmpty = true;
            }

            Busy = false;
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

            GetMiniStudentViewAttendants();
        }

        // Ready
        private void HandlePostAttendant()
        {
            //if (StudyGroupView.Date >= DateTime.Now && StudyGroupView.Time > DateTime.Now)
                Task.WaitAll(PostAttendanceAsync());
                IsAttendant = true;
            AttendButtonCaption = "Leave";
        }

        // Ready
        private void HandleRemoveAttendant()
        {
            //if(StudyGroupView.Date <= DateTime.Now && StudyGroupView.Time < DateTime.Now)
                RemoveAttendanceAsync();
                IsAttendant = false;
            AttendButtonCaption = "Join";
        }

        // Ready
        private async Task PostAttendanceAsync()
        {
            //AttendButtonPlaying = true;

            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants";

            var sga = new StudyGroupAttendants { StudentId = StudentId, StudyGroupId = StudyGroupView.StudyGroupId };

            var content = new StringContent(JsonConvert.SerializeObject(sga), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var StudyGroupInserted = JsonConvert.DeserializeObject<StudyGroupAttendants>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                //await _pageService.DisplayAlert("Add Attendant", "You have been added to the Group. ", "OK", "Cancel");
            }
            else
            {
                //await _pageService.DisplayAlert("Error", "Something went wrong with Add Study Group attendant", "OK", "Cancel");
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

        public void HandlePostNewCommand()
        {
            if (NewComment.Comment == null) return;
            Task.WaitAll(PostNewComment());
            NewComment = new StudyGroupComment();
            GetComments();
        }

        private async Task PostNewComment()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupComments";

            NewComment.StudentId = StudentId;
            NewComment.StudyGroupId = StudyGroupView.StudyGroupId;
            NewComment.FullTime = DateTime.Now;

            var content = new StringContent(JsonConvert.SerializeObject(NewComment), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var StudyGroupInserted = JsonConvert.DeserializeObject<StudyGroupComment>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                //await _pageService.DisplayAlert("Add Attendant", "You have been added to the Group. ", "OK", "Cancel");
            }
            else
            {
                //await _pageService.DisplayAlert("Error", "Something went wrong with Add Study Group attendant", "OK", "Cancel");
            }

        }
    }
}
