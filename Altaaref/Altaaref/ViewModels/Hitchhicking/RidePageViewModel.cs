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

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
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

        private bool _isInvitButtonVisible;
        public bool IsInvitButtonVisible
        {
            get { return _isInvitButtonVisible; }
            set
            {
                SetValue(ref _isInvitButtonVisible, value);
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

        private RideComments _newComment;
        public RideComments NewComment
        {
            get { return _newComment; }
            set
            {
                _newComment = value;
                OnPropertyChanged(nameof(NewComment));
            }
        }

        private List<RideComments> _rideComments;
        public List<RideComments> RideComments
        {
            get { return _rideComments; }
            set
            {
                _rideComments = value;
                OnPropertyChanged(nameof(RideComments));
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

        private bool _isDeleteButtonVisible;
        public bool IsDeleteButtonVisible
        {
            get { return _isDeleteButtonVisible; }
            set { SetValue(ref _isDeleteButtonVisible, value); }
        }

        public ICommand ViewProfileCommand => new Command(async () => await HandleViewProfile());
        public ICommand SendAttendantCommand => new Command(async () => await HandleSendAttendant());
        public ICommand MessageDriver => new Command(async () => await HandleMessageDriver());
        public ICommand PostButtonCommand => new Command(async () => await AddComment());
        public ICommand DeleteCommand => new Command(async () => await HandleDelete());

        public RidePageViewModel(Ride ride, IPageService pageService)
        {
            _pageService = pageService;

            NewComment = new RideComments();

            Ride = ride;

            if (Settings.StudentId == ride.DriverId)
            {
                IsInvitButtonVisible = false;
            }
            else
            {
                IsInvitButtonVisible = true;
                var initcond = GetIsSentInvitation();
            }

            var url = "https://altaarefapp.azurewebsites.net/api/RideAttendants/ByRideId/" + Ride.Id;

            var results = _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<RideAttendants>>(results.Result);
            Attendants = list;

            if (Attendants.Count > 0)
                IsAttendantsEmpty = false;
            else
                IsAttendantsEmpty = true;

            NewComment = new RideComments
            {
                RideId = ride.Id,
                StudentId = Settings.StudentId
            };

            var comments = GetComments();

            if (ride.DriverId == Settings.StudentId)
                IsDeleteButtonVisible = true;
            else
                IsDeleteButtonVisible = false;
        }

        private async Task HandleDelete()
        {
            var results = await _pageService.DisplayAlert("Are you sure ?", "Are you sure you want to cancel this ride ?", "OK", "Cancel");

            if (!results) return;

            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Rides/" + Ride.Id;

            try
            {
                var content = await _client.DeleteAsync(url);

                Busy = false;
            }
            catch (HttpRequestException e)
            {
                Busy = false;
            }

            Busy = false;
        }

        private async Task GetComments()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/RideComments/GetByRideId/" + Ride.Id;

            try
            {
                var content = await _client.GetStringAsync(url);
                var ridcomments = JsonConvert.DeserializeObject<List<RideComments>>(content);
                RideComments = ridcomments;

                if (RideComments != null && RideComments.Count != 0)
                    CommentsEmpty = false;
                else
                    CommentsEmpty = true;
            }
            catch (HttpRequestException e)
            {
                CommentsEmpty = true;
                Busy = false;
            }

            Busy = false;

        }

        private async Task AddComment()
        {
            if (NewComment.Comment == null) return;
            PostNewComment();

            await GetComments();

            NewComment = new RideComments
            {
                StudentId = Settings.StudentId,
                RideId = Ride.Id,
                Comment = ""
            };
        }

        private async void PostNewComment()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/RideComments";

            var content = new StringContent(JsonConvert.SerializeObject(NewComment), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            if (response.Result.IsSuccessStatusCode)
            {
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with commenting", "OK", "Cancel");
            }
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
                await DeleteMeFromAttendants();

                Ride.RideAttendants.Remove(Ride.RideAttendants.Find(r => r.AttendantId == Settings.StudentId));
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

            try
            {
                string content = await _client.GetStringAsync(url);
                if (content != null)
                    IssendButtonVisible = false;
                else
                    IssendButtonVisible = true;
            }
            catch(HttpRequestException e)
            {
                IssendButtonVisible = true;
            }
//            Busy = false;
        }

        private async Task DeleteMeFromAttendants()
        {
            var url = "https://altaarefapp.azurewebsites.net/api/RideAttendants/" + Settings.StudentId + "/" + Ride.Id;

            try
            {
                var content = await _client.DeleteAsync(url);
            }
            catch (HttpRequestException e)
            {
            }
        }

    }
}
