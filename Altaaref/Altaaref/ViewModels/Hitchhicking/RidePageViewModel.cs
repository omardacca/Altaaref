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

namespace Altaaref.ViewModels.Hitchhicking
{
    public class RidePageViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private Ride _ride;
        public Ride Ride
        {
            get { return _ride; }
            private set
            {
                _ride = value;
                OnPropertyChanged(nameof(Ride));
            }
        }

        private List<RideAttendants> _attendants;
        public List<RideAttendants> Attendants
        {
            get { return _attendants; }
            set
            {
                _attendants = value;
                OnPropertyChanged(nameof(Attendants));
            }
        }

        private bool _isAttendantsEmpty;
        public bool IsAttendantsEmpty
        {
            get { return _isAttendantsEmpty; }
            set
            {
                SetValue(ref _isAttendantsEmpty, value);
            }
        }

        private bool _IssendButtonVisible;
        public bool IssendButtonVisible
        {
            get { return _IssendButtonVisible; }
            set
            {
                SetValue(ref _IssendButtonVisible, value);
            }
        }

        public ICommand ViewProfileCommand => new Command(async () => await HandleViewProfile());
        public ICommand SendAttendantCommand => new Command(async () => await HandleSendAttendant());
        public ICommand MessageDriver => new Command(async () => await HandleMessageDriver());

        public RidePageViewModel(Ride ride, IPageService pageService)
        {
            _pageService = pageService;

            Ride = ride;

            if (Settings.StudentId == ride.DriverId)
            {
                IssendButtonVisible = true;
            }
            else
            {
                var initcond = GetIsSentInvitation();
            }

            var url = "https://altaarefapp.azurewebsites.net/api/RideAttendants/ByRideId/" + Ride.Id;

            var results = _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<RideAttendants>>(results.Result);
            Attendants = list;

            Ride.NumOfFreeSeats -= byte.Parse(Attendants.Count.ToString());

            if (Attendants.Count > 0)
                IsAttendantsEmpty = false;
            else
                IsAttendantsEmpty = true;
        }

        private async Task HandleMessageDriver()
        {

        }

        private async Task HandleSendAttendant()
        {
            if(IssendButtonVisible)
            {
                // if successfull, should return false
                IssendButtonVisible = await PostRequest();
            }
            else
            {
                IssendButtonVisible  = await DeleteRequest();
            }
        }

        private async Task HandleViewProfile()
        {

        }

        private async Task<bool> DeleteRequest()
        {
            var url = "https://altaarefapp.azurewebsites.net/api/RidesInvitations/" + Ride.Id + "/" + Settings.StudentId;

            try
            {
                var content = await _client.DeleteAsync(url);
                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        private async Task<bool> PostRequest()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/RidesInvitations";

            RidesInvitations invitation = new RidesInvitations
            {
                RideId = Ride.Id,
                CandidateId = Settings.StudentId
            };
                
            var content = new StringContent(JsonConvert.SerializeObject(invitation), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            if (response.Result.IsSuccessStatusCode)
            {
                //await _pageService.DisplayAlert("Students Invited", "Students Invited Successfully", "OK", "Cancel");

                return false;
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with Posting invitation", "OK", "Cancel");
                return true;
            }

        }

        private async Task GetIsSentInvitation()
        {
//            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/RidesInvitations/" + Ride.Id + "/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<RidesInvitations>>(content);

            if (list.Count == 0)
                IssendButtonVisible = true;
            else
                IssendButtonVisible = false;
            //            Busy = false;
        }

    }
}
