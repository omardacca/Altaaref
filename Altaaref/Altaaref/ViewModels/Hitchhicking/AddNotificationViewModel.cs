using Altaaref.Helpers;
using Altaaref.Models;
using DurianCode.PlacesSearchBar;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.Hitchhicking
{
    public class AddNotificationViewModel : BaseViewModel
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
        public ICommand SubscribeButtonTap => new Command(async() => await HandleSubscribeButtonTap());

        public AddNotificationViewModel(IPageService pageService)
        {
            _pageService = pageService;
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

                var place = await Places.GetPlace(prediction.Place_ID, App.GooglePlacesApi);

                NewRide.ToLong = place.Longitude;
                NewRide.ToLat = place.Latitude;
                NewRide.ToCity = place.Name;
            }

        }

        private async Task HandleSubscribeButtonTap()
        {
            if(NewRide.FromCity != null && NewRide.ToCity != null)
            {
                // Subscribe to topic
                DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("RP" + NewRide.FromCity.Trim() + NewRide.ToCity.Trim());

                // Register StudentId with the notification in the db, and update the list in App Properties
                var insertedUserNotification = await FCMPushNotificationSender.AddNotification(
                    new UserNotification
                    {
                        StudentId = Settings.StudentId,
                        Topic = "RP" + NewRide.FromCity.Trim() + NewRide.ToCity.Trim(),
                        Title = "New Ride",
                        Body = "Someone published new ride that you interested in"
                    });

            }
        }
    }
}
