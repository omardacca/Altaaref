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
    public class NewHelpRequestViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private Models.HelpRequest _newHelpRequest;
        public Models.HelpRequest NewHelpRequest
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
            NewHelpRequest = new Models.HelpRequest
            {
                StudentId = StudentId,
                Message = "",
                IsGeneral = false
            };
        }

        private async void OnSubmitButtonTapped(object obj)
        {
            if(NewHelpRequest.Message.Trim() != "")
            {
                // if request not general - show next page to pick Faculties
                if (!NewHelpRequest.IsGeneral)
                {
                    await _pageService.PushAsync(new Views.MutualHelp.SelectHelpRequestFaculties(NewHelpRequest));
                }
                else
                {
                    await PostGeneralHelpRequest();
                }
            }
            else
                await _pageService.DisplayAlert("Invalid Entry", "Please fill all form correctly", "OK", "Cancel");

        }

        private async Task<int> PostGeneralHelpRequest()
        {
            Busy = true;

            var postUrl = "https://altaarefapp.azurewebsites.net/api/HelpRequests";

            var content = new StringContent(JsonConvert.SerializeObject(NewHelpRequest), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var InsertedHelpRequestId = JsonConvert.DeserializeObject<Models.HelpRequest>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Created Successfully", "Help Request Created Successfully", "OK", "Cancel");
                Busy = false;
                return InsertedHelpRequestId.Id;
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
                Busy = false;
                return -1;
            }


        }
    }
}
