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

        ICommand metImageCommand;
        public ICommand MetImageCommand { get => metImageCommand;}

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

        public ViewMyHelpRequestsDetailsViewModel(IPageService pageService, StudentHelpRequest studentHelpRequest)
        {
            _pageService = pageService;
            this.StudentHelpRequest = studentHelpRequest;

            metImageCommand = new Command(HandleMetImageTap);
        }
    }
}
