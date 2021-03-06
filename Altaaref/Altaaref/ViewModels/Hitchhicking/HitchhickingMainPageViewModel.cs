﻿using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.Hitchhicking
{
    public class HitchhickingMainPageViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private string _lat;
        public string Lat
        {
            get { return _lat; }
            set
            {
                SetValue(ref _lat, value);
            }
        }

        private string _long;
        public string Long
        {
            get { return _long; }
            set
            {
                SetValue(ref _long, value);
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

        private Plugin.Geolocator.Abstractions.Position _position;

        private List<Ride> _myRidesList;
        public List<Ride> MyRidesList
        {
            get { return _myRidesList; }
            set
            {
                _myRidesList = value;
                OnPropertyChanged(nameof(MyRidesList));
            }
        }

        private List<Ride> _nearbyRidesList;
        public List<Ride> NearbyRidesList
        {
            get { return _nearbyRidesList; }
            set { SetValue(ref _nearbyRidesList, value); }
        }
        
        public ICommand GetLoc => new Command(async () => await GetLocation());
        public ICommand AddRideCommand => new Command(async () => await NavigateToAddRide());
        public ICommand FindRideCommand => new Command(async () => await NavigateToFindRide());
        public ICommand AddNotifCommand => new Command(async () => await NavigateToAddNotif());
        public ICommand ItemTappedCommand => new Command<Ride>(async (ride) => await HandleItemTapped(ride));

        private bool _isMyRidesListEmpty;
        public bool IsMyRidesListEmpty
        {
            get { return _isMyRidesListEmpty; }
            set { SetValue(ref _isMyRidesListEmpty, value); }
        }

        private bool _isNearbyListEmpty;
        private bool IsNearbyListEmpty
        {
            get { return _isNearbyListEmpty; }
            set { SetValue(ref _isNearbyListEmpty, value); }
        }

        public HitchhickingMainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            Busy = true;

            var lat = Application.Current.Properties["Latitude"].ToString();
            var longtitud = Application.Current.Properties["Longtitude"].ToString();
            if (lat == null || lat == "" || longtitud == null || longtitud == "")
            {
                var x = GetLocation();
            }

            _position = new Plugin.Geolocator.Abstractions.Position(double.Parse(lat), double.Parse(longtitud));

            var init = InitLists();

            Busy = false;
        }

        private async Task InitLists()
        {
            await GetMyRidesList();

            await GetNearbyList();
        }

        private async Task HandleItemTapped(Ride ride)
        {
            await _pageService.PushAsync(new Views.Hitchhicking.RidePage(ride));
        }

        private async Task GetMyRidesList()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/Rides/GetStudentRides/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Ride>>(content);
            var rides = new List<Ride>(list);
            MyRidesList = new List<Ride>(rides);

            if (rides.Count != 0)
            {
                IsMyRidesListEmpty = false;
                FixNumOfSeats(MyRidesList);
            }
            else
                IsMyRidesListEmpty = true;

            Busy = false;
        }

        private void FixNumOfSeats(List<Ride> RidesList)
        {
            foreach(var item in RidesList)
            {
                item.NumOfFreeSeats = byte.Parse((item.NumOfFreeSeats - item.RideAttendants.Count).ToString());
            }
        }

        private async Task GetNearbyList()
        {
            Busy = true;

            // should wait or repeat until we get position
            if (_position == null)
            {
                Busy = false;
                return;
            }

            string url = "https://altaarefapp.azurewebsites.net/api/Rides/GetNearbyRides/";
            var place = _position.Longitude.ToString() + "/" + _position.Latitude.ToString();

            url = url + place;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Ride>>(content);
            var rides = new List<Ride>(list);
            NearbyRidesList = new List<Ride>(rides);

            if (rides.Count != 0)
            {
                IsNearbyListEmpty = false;
                FixNumOfSeats(NearbyRidesList);
            }
            else
                IsNearbyListEmpty = true;

            Busy = false;
        }

        private async Task GetLocation()
        {
            var hasPermission = await Utils.CheckPermissions(Permission.Location);

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;

            _position = await locator.GetPositionAsync();

            //Lat = "Latitude: " + position.Latitude.ToString();
            //Long = "Longtitude: " + position.Longitude.ToString();
        }

        private async Task NavigateToAddNotif()
        {
            await _pageService.PushAsync(new Views.Hitchhicking.AddNotification());
        }

        private async Task NavigateToAddRide()
        {
            await _pageService.PushAsync(new Views.Hitchhicking.AddRide());
        }

        private async Task NavigateToFindRide()
        {
            await _pageService.PushAsync(new Views.Hitchhicking.FindRide());
        }

    }
}
