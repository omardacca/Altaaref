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
    public class MyRidesRequestsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<Ride> _ridesList;
        public List<Ride> RidesList
        {
            get { return _ridesList; }
            private set
            {
                _ridesList = value;
                OnPropertyChanged(nameof(RidesList));
            }
        }

        public ICommand ViewInvitationsCommand => new Command(async () => await HandleViewInvitations());

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


        public MyRidesRequestsViewModel(IPageService pageService)
        {
            _pageService = pageService;

            var getrides = GetRides();
        }

        private async Task GetRides()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Rides/GetStudentRides/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Ride>>(content);
            RidesList = new List<Ride>(list);

            if (RidesList == null || RidesList.Count == 0)
                IsRidesListEmpty = true;

            Busy = false;
        }

        private async Task HandleViewInvitations()
        {
            //await _pageService.PushAsync(new Views.Hitchhicking.RideInvitations());
        }

    }
}
