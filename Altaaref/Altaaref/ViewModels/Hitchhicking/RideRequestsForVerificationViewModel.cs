﻿using Altaaref.Models;
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
    public class RideRequestsForVerificationViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<RidesInvitations> _requestsList;
        public List<RidesInvitations> RequestsList
        {
            get { return _requestsList; }
            set
            {
                _requestsList = value;
                OnPropertyChanged(nameof(RequestsList));
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

        private bool _isInvitationsListEmpty;
        public bool IsInvitationsListEmpty
        {
            get { return _isInvitationsListEmpty; }
            set
            {
                SetValue(ref _isInvitationsListEmpty, value);
            }
        }

        private Ride _ride;

        public ICommand ItemTappedCommand => new Command<RidesInvitations>(async (rideinv) => await HandleInvitationTap(rideinv));

        public RideRequestsForVerificationViewModel(IPageService pageService, Ride ride)
        {
            _pageService = pageService;

            _ride = ride;

            var getrides = GetRideInvitaions();

        }

        private async Task HandleInvitationTap(RidesInvitations rideinv)
        {
            if (!rideinv.Status)
            {
                // get the current number of attendants
                var count = await GetNumberOfAttendants(rideinv.Ride);

                if (rideinv.Ride.NumOfFreeSeats - count == 0)
                {
                    await _pageService.DisplayAlert("Oops!", "Sorry, no more free seats lef!", "Ok", "Cancel");
                    return;
                }
            }
            Busy = true;

            rideinv.Status = !rideinv.Status;
            bool resultput = await PutInvitationStatus(rideinv);

            var passed = true;
            if (rideinv.Status)
                passed = await PostRideAttendant(rideinv);
            else
                passed = await DeleteAttendant(rideinv);


            if (!resultput || !passed)
                rideinv.Status = !rideinv.Status;
            else
                RequestsList.Find(ride => ride.RideId == rideinv.RideId && ride.CandidateId == rideinv.CandidateId).Status = rideinv.Status;
            Busy = false;
        }

        private async Task<bool> DeleteAttendant(RidesInvitations rideinv)
        {
            Busy = true;

            var url = "https://altaarefapp.azurewebsites.net/api/RideAttendants/" + rideinv.CandidateId + "/" + rideinv.RideId;

            var response = await _client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Busy = false;
                return true;
            }
            else
            {
                Busy = false;
                return false;
            }

        }

        private async Task<int> GetNumberOfAttendants(Ride ride)
        {
            var url = "https://altaarefapp.azurewebsites.net/api/Rides/GetNumberOfAttendants/" + ride.Id;
            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<int>>(results);
            int count = int.Parse(list[0].ToString());
            return count;
        }

        private async Task GetRideInvitaions()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/RidesInvitations/GetRidesInvitationsByRideId/" + _ride.Id;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<RidesInvitations>>(content);
            RequestsList = new List<RidesInvitations>(list);

            if (RequestsList == null || RequestsList.Count == 0)
                IsInvitationsListEmpty = true;

            Busy = false;
        }

        private async Task<bool> PutInvitationStatus(RidesInvitations UpdatedRideInvitaion)
        {
            var puttUrl = "https://altaarefapp.azurewebsites.net/api/RidesInvitations/" + UpdatedRideInvitaion.RideId + "/" + UpdatedRideInvitaion.CandidateId;

            var content = new StringContent(JsonConvert.SerializeObject(UpdatedRideInvitaion), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(puttUrl, content);

            return response.IsSuccessStatusCode;
        }

        private async Task<bool> PostRideAttendant(RidesInvitations UpdatedRideInvitaion)
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/RideAttendants/";

            RideAttendants rideAttendants = new RideAttendants
            {
                AttendantId = UpdatedRideInvitaion.CandidateId,
                RideId = UpdatedRideInvitaion.RideId
            };

            var content = new StringContent(JsonConvert.SerializeObject(rideAttendants), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var insertedRes = await response.Result.Content.ReadAsStringAsync();

            if (response.Result.IsSuccessStatusCode)
            {
                //await FCMPushNotificationSender.Send(
                //    "SG" + StudyGroup.CourseId,
                //    "New Study Group",
                //    "New study group added in a course you interested in, join now!");

                return true;
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
                return false;
            }

        }
    }
}
