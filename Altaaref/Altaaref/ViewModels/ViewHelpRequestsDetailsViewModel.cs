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
    public class StudentHelpComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public Student Student { get; set; }
    }

    public class ViewHelpRequestsDetailsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public StudentHelpRequest StudentHelpRequest { get; set; }

        private HelpRequestComment _newComment;
        public HelpRequestComment NewComment
        {
            get { return _newComment; }
            set
            {
                _newComment = value;
                OnPropertyChanged(nameof(NewComment));
            }
        }

        private List<StudentHelpComment> _allComments;
        public List<StudentHelpComment> AllComments
        {
            get { return _allComments; }
            set
            {
                _allComments = value;
                OnPropertyChanged(nameof(AllComments));
            }
        }

        private StudentHelpComment _selectedComment;
        public StudentHelpComment SelectedComment
        {
            get { return _selectedComment; }
            set { SetValue(ref _selectedComment, value); }
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

        ICommand metImageCommand;
        public ICommand MetImageCommand { get => metImageCommand;}

        ICommand postButtonCommand;
        public ICommand PostButtonCommand { get => postButtonCommand; }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        public ViewHelpRequestsDetailsViewModel(IPageService pageService, StudentHelpRequest studentHelpRequest)
        {
            _pageService = pageService;
            this.StudentHelpRequest = studentHelpRequest;

            CommentsEmpty = false;

            this.NewComment = new HelpRequestComment
            {
                HelpRequestId = this.StudentHelpRequest.Id
            };

            GetComments();

            metImageCommand = new Command(HandleMetImageTap);
            postButtonCommand = new Command(AddComment);
        }

        private void HandleMetImageTap(object s)
        {
            StudentHelpRequest.IsMet = !StudentHelpRequest.IsMet;

            PutIsMet();
        }

        private void PutIsMet()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequests/" + StudentHelpRequest.Id;

            HelpRequest updated = new HelpRequest
            {
                Id = StudentHelpRequest.Id,
                IsGeneral = StudentHelpRequest.IsGeneral,
                Date = StudentHelpRequest.Date, //ooo
                IsMet = StudentHelpRequest.IsMet,
                Message = StudentHelpRequest.Message,
                Views = StudentHelpRequest.Views,
                StudentId = StudentHelpRequest.Student.Id
            };

            var content = new StringContent(JsonConvert.SerializeObject(updated), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(url, content).Result;
            
            Busy = false;
        }

        private async void GetComments()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/HelpRequestComments/getComments/" + StudentHelpRequest.Id;

            string content = await _client.GetStringAsync(url);
            var commentslist = JsonConvert.DeserializeObject<List<StudentHelpComment>>(content);
            AllComments = commentslist;

            if (AllComments != null && AllComments.Count != 0)
                CommentsEmpty = false;
            else
                CommentsEmpty = true;

            Busy = false;
        }
        
        private void AddComment()
        {
            if (NewComment.Comment == null) return;
            PostNewComment();
            this.NewComment = new HelpRequestComment
            {
                HelpRequestId = this.StudentHelpRequest.Id
            };

            //AllComments.Add(new StudentHelpComment { Id = NewComment.Id, Comment = NewComment.Comment, Student = StudentHelpRequest.Student });

            // Temporar solution.. above commented line isn't working..
            GetComments();
        }

        private async void PostNewComment()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/HelpRequestComments";

            var content = new StringContent(JsonConvert.SerializeObject(NewComment), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Comment Posted", "Comment Posted Successfully", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with commenting", "OK", "Cancel");
            }
        }

        public void HanldeCommentTapped(StudentHelpComment comment)
        {
        }
    }
}
