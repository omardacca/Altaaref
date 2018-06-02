using Altaaref.Helpers;
using Altaaref.Models;
using DurianCode.PlacesSearchBar;
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
    public class FindRideViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<AutoCompletePrediction> _fromAutoCompletePredictions;
        public List<AutoCompletePrediction> FromAutoCompletePredictions
        {
            get { return _fromAutoCompletePredictions; }
            set
            {
                _fromAutoCompletePredictions = value;
                OnPropertyChanged(nameof(FromAutoCompletePredictions));
            }
        }

        private List<AutoCompletePrediction> _toAutoCompletePredictions;
        public List<AutoCompletePrediction> ToAutoCompletePredictions
        {
            get { return _toAutoCompletePredictions; }
            set
            {
                _toAutoCompletePredictions = value;
                OnPropertyChanged(nameof(ToAutoCompletePredictions));
            }
        }

        private AutoCompletePrediction _selectedFromPlace;
        private AutoCompletePrediction _selectedToPlace;

        private List<Ride> _ridesList;
        public List<Ride> RidesList
        {
            get { return _ridesList; }
            set
            {
                _ridesList = value;
                OnPropertyChanged(nameof(RidesList));
            }
        }

        private bool _isRidesListEmpty;
        public bool IsRidesListEmpty
        {
            get { return _isRidesListEmpty; }
            set
            {
                SetValue(ref _isRidesListEmpty, value);
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

        private bool _isDateOn;
        public bool IsDateOn
        {
            get { return _isDateOn; }
            set
            {
                SetValue(ref _isDateOn, value);
            }
        }

        private bool _isTimeOn;
        public bool IsTimeOn
        {
            get { return _isTimeOn; }
            set
            {
                SetValue(ref _isTimeOn, value);
            }
        }

        private string _fromText;
        public string FromText
        {
            get { return _fromText; }
            set
            {
                SetValue(ref _fromText, value);
            }
        }

        private string _toText;
        public string ToText
        {
            get { return _toText; }
            set
            {
                SetValue(ref _toText, value);
            }
        }

        private bool _isFromVisible;
        public bool IsFromVisible
        {
            get { return _isFromVisible; }
            set
            {
                SetValue(ref _isFromVisible, value);
            }
        }

        private bool _isToVisible;
        public bool IsToVisible
        {
            get { return _isToVisible; }
            set
            {
                SetValue(ref _isToVisible, value);
            }
        }

        private Ride _searchRide;
        public Ride SearchRide
        {
            get { return _searchRide; }
            set
            {
                SetValue(ref _searchRide, value);
            }
        }

        public ICommand AutocompletePlaceTap => new Command<AutoCompletePrediction>(async (Prediction) => await HandlePlacePredictionTap(Prediction));
        public ICommand SearchButtonTapCommand => new Command(async () => await HandleSearchButtonTap());
        public ICommand DateToggleCommand => new Command(() => HandleDateToggle());
        public ICommand TimeToggleCommand => new Command(() => HandleTimeToggle());
        public ICommand ItemTappedCommand => new Command<Ride>(async(ride) => await HandleResultItemTap(ride));


        public FindRideViewModel(IPageService pageService)
        {
            _pageService = pageService;
            IsRidesListEmpty = true;
            SearchRide = new Ride
            {
                Date = DateTime.Now,
                Time = DateTime.Now
            };

        }

        private async Task HandleResultItemTap(Ride ride)
        {
            await _pageService.PushAsync(new Views.Hitchhicking.RidePage(ride));
        }

        private void HandleDateToggle()
        {
            //IsDateOn = !IsDateOn;
        }

        private void HandleTimeToggle()
        {
            //IsTimeOn = !IsTimeOn;
        }

        public void SearchBarPlacesRetrieved(AutoCompleteResult result, string FromOrTo)
        {
            if (FromOrTo == "from")
            {
                FromAutoCompletePredictions = result.AutoCompletePlaces;
                IsFromVisible = true;
            }
            else
            {
                ToAutoCompletePredictions = result.AutoCompletePlaces;
                IsToVisible  = true;
            }
        }

        private async Task HandlePlacePredictionTap(AutoCompletePrediction prediction)
        {
            if (FromAutoCompletePredictions != null)
            {
                IsFromVisible = false;
                _selectedFromPlace = prediction;
                FromAutoCompletePredictions = null;

                FromText = prediction.Description;

                var place = await Places.GetPlace(prediction.Place_ID, App.GooglePlacesApi);
                SearchRide.FromLong = place.Longitude;
                SearchRide.FromLat = place.Latitude;
                SearchRide.FromCity = place.Name;
            }
            else
            {
                IsToVisible = false;
                _selectedToPlace = prediction;
                ToAutoCompletePredictions = null;

                ToText = prediction.Description;

                var place = await Places.GetPlace(prediction.Place_ID, App.GooglePlacesApi);

                SearchRide.ToLong = place.Longitude;
                SearchRide.ToLat = place.Latitude;
                SearchRide.ToCity = place.Name;
            }

        }

        private async Task HandleSearchButtonTap()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Rides/";

            if (_selectedFromPlace == null || _selectedToPlace == null)
            {
                await _pageService.DisplayAlert("Error", "Please fill the form properly.", "Ok", "Cancel");
                return;
            }

            var place = SearchRide.FromLong.ToString() + "/" + SearchRide.FromLat.ToString() + "/" + SearchRide.ToLong + "/" + SearchRide.ToLat.ToString() + "/";

            if (_isDateOn && IsTimeOn)
                url += "GetWithDateTime/" + place + SearchRide.Date.ToString("yyyy-MM-dd") + " / " + SearchRide.Time.ToString("HH:mm");
            else if (_isDateOn)
                url += "GetWithDateTime/" + place + SearchRide.Date.ToString("yyyy-MM-dd");
            else if (_isTimeOn)
                url += "GetWithDateTime/" + place + SearchRide.Time.ToString("HH:mm");
            else
                url += "GetWithoutDate/" +  place;

            await GetSearchResults(url);
        }

        private void UpdateListForView()
        {
            foreach(var ride in RidesList)
            {
                ride.NumOfFreeSeats -= byte.Parse(ride.RideAttendants.Count.ToString());
            }
        }

        private async Task GetSearchResults(string filteredUrl)
        {
            Busy = true;

            string results = await _client.GetStringAsync(filteredUrl);
            var list = JsonConvert.DeserializeObject<List<Ride>>(results);
            RidesList = new List<Ride>(list);

            UpdateListForView();

            if (RidesList == null || RidesList.Count == 0)
                IsRidesListEmpty = true;
            else
                IsRidesListEmpty = false;

            Busy = false;
        }

    }
}
