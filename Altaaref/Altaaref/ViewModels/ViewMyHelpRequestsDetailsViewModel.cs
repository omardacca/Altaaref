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
    public class ViewMyHelpRequestsDetailsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public StudentHelpRequest StudentHelpRequest { get; set; }
        public HelpRequestComment NewComment { get; set; }


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
                IsMet = StudentHelpRequest.IsMet,
                Message = StudentHelpRequest.Message,
                Views = StudentHelpRequest.Views,
                StudentId = StudentHelpRequest.Student.Id
            };

            var content = new StringContent(JsonConvert.SerializeObject(updated), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(url, content).Result;
            
            Busy = false;
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

        public ViewMyHelpRequestsDetailsViewModel(IPageService pageService, StudentHelpRequest studentHelpRequest)
        {
            _pageService = pageService;
            this.StudentHelpRequest = studentHelpRequest;

            this.NewComment = new HelpRequestComment
            {
                HelpRequestId = this.StudentHelpRequest.Id
            };

            metImageCommand = new Command(HandleMetImageTap);
            postButtonCommand = new Command(PostNewComment);
        }
    }
}
