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
    public class NewHelpRequestViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private HelpRequest _newHelpRequest;
        public HelpRequest NewHelpRequest
        {
            get { return _newHelpRequest; }
            private set { SetValue(ref _newHelpRequest, value); }
        }

        public ICommand HandleSubmit{ get; private set; }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        public NewHelpRequestViewModel(IPageService pageService)
        {
            _pageService = pageService; 

            InitNewHelpRequest();

            HandleSubmit = new Command(OnSubmitButtonTapped);
        }

        private void InitNewHelpRequest()
        {
            NewHelpRequest = new HelpRequest();
            NewHelpRequest.StudentId = StudentId;
            NewHelpRequest.Message = "";
            NewHelpRequest.IsGeneral = false;
        }

        private async void OnSubmitButtonTapped(object obj)
        {
            if(NewHelpRequest.Message.Trim() != "")
            {
                // if request not general - show next page to pick Faculties
                if (!NewHelpRequest.IsGeneral)
                {

                }
                else
                {
                    PostGeneralHelpRequest();
                }
            }
            else
                await _pageService.DisplayAlert("Invalid Entry", "Please fill all form correctly", "OK", "Cancel");

        }

        private async void PostGeneralHelpRequest()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/HelpRequests/";

            var content = new StringContent(JsonConvert.SerializeObject(NewHelpRequest), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var InstertedHelpRequest = JsonConvert.DeserializeObject<HelpRequest>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
                await _pageService.DisplayAlert("Created Successfully", "Help Request Created Successfully", "OK", "Cancel");
            else
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");

        }
    }
}
