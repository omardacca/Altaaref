using Altaaref.Helpers;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using DurianCode.PlacesSearchBar;
using Altaaref.Models;
using Newtonsoft.Json;

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
        
        private Ride _newRide;
        public Ride NewRide
        {
            get { return _newRide; }
            set
            {
                SetValue(ref _newRide, value);
            }
        }

        public ICommand GetLoc => new Command(async () => await GetLocation());
        public ICommand AutocompletePlaceTap => new Command<AutoCompletePrediction>(async(Prediction) => await HandlePlacePredictionTap(Prediction));
        public ICommand SubmitCommand => new Command(async () => await HandleSubmition());


        public HitchhickingMainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
            _newRide = new Ride
            {
                Date = DateTime.Now,
                Time = DateTime.Now
            };
        }

        private async Task GetLocation()
        {
            var hasPermission = await Utils.CheckPermissions(Permission.Location);

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;

            var position = await locator.GetPositionAsync();

            Lat = "Latitude: " + position.Latitude.ToString();
            Long = "Longtitude: " + position.Longitude.ToString();
        }

        public void SearchBarPlacesRetrieved(AutoCompleteResult result, string FromOrTo)
        {
            if(FromOrTo == "from")
            {
                FromAutoCompletePredictions = result.AutoCompletePlaces;
                IsFromVisible = true;
            }
            else
            {
                ToAutoCompletePredictions = result.AutoCompletePlaces;
                IsToVisible = true;
            }
        }

        private async Task HandlePlacePredictionTap(AutoCompletePrediction prediction)
        {
            if(FromAutoCompletePredictions != null)
            {
                IsFromVisible = false;
                _selectedFromPlace = prediction;
                FromAutoCompletePredictions = null;

                FromText = prediction.Description;

                var place = await Places.GetPlace(prediction.Place_ID, App.GooglePlacesApi);
                NewRide.FromLong = place.Longitude;
                NewRide.FromLat = place.Latitude;
                NewRide.FromCity = place.Name;
            }
            else
            {
                IsToVisible = false;
                _selectedToPlace = prediction;
                ToAutoCompletePredictions = null;

                ToText = prediction.Description;

                var place = await Places.GetPlace(prediction.Place_ID, App.GooglePlacesApi);

                NewRide.ToLong = place.Longitude;
                NewRide.ToLat = place.Latitude;
                NewRide.ToCity = place.Name;
            }

        }

        private async Task HandleSubmition()
        {
            NewRide.DriverId = Settings.StudentId;

            await PostRide();
        }

        private async Task PostRide()
        {
            Busy = true;

            var postUrl = "https://altaarefapp.azurewebsites.net/api/Rides";

            var content = new StringContent(JsonConvert.SerializeObject(NewRide), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var InsertedRide = JsonConvert.DeserializeObject<Ride>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                //await _pageService.DisplayAlert("Created Successfully", "Ride Created Successfully", "OK", "Cancel");
                Busy = false;
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong while adding the ride", "OK", "Cancel");
                Busy = false;
            }


        }

    }
}
