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
    public class AddRideViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

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

        public ICommand AutocompletePlaceTap => new Command<AutoCompletePrediction>(async (Prediction) => await HandlePlacePredictionTap(Prediction));
        public ICommand SubmitCommand => new Command(async () => await HandleSubmition());


        public AddRideViewModel(IPageService pageService)
        {
            _pageService = pageService;

            _newRide = new Ride
            {
                Date = DateTime.Now
            };

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
                IsToVisible = true;
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
            Busy = true;
            NewRide.DriverId = Settings.StudentId;

            var insertedRide = await PostRide();

            if(insertedRide != null)
            {
                await _pageService.PushAsync(new Views.Hitchhicking.RidePage(insertedRide));
                Busy = false;
            }

            Busy = false;

        }

        private async Task<Ride> PostRide()
        {
            Busy = true;

            var postUrl = "https://altaarefapp.azurewebsites.net/api/Rides";

            var content = new StringContent(JsonConvert.SerializeObject(NewRide), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);
            var InsertedRide = JsonConvert.DeserializeObject<Ride>(await response.Result.Content.ReadAsStringAsync());

            if (response.Result.IsSuccessStatusCode)
            {
                // Send Notification


                var datestring = NewRide.Date.ToString("MMdd");
                datestring += NewRide.Date.ToString("HHmm");

                await FCMPushNotificationSender.Send(
                    "HI" + NewRide.FromCity.Trim() + "To" + NewRide.ToCity.Trim() + datestring,
                    "New Ride",
                    "New ride to " + NewRide.ToCity + " was added!");

                Busy = false;

                return InsertedRide;
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong while adding the ride", "OK", "Cancel");
                Busy = false;

                return null;
            }


        }
    }
}
