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
                OnPropertyChanged(nameof(RidesInvitations));
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

        private int RideId;

        public ICommand ItemTappedCommand => new Command<RidesInvitations>(async (rideinv) => await HandleInvitationTap(rideinv));


        public RideRequestsForVerificationViewModel(IPageService pageService, int rideId)
        {
            _pageService = pageService;

            RideId = rideId;

            var getrides = GetRideInvitaions();

        }

        private async Task HandleInvitationTap(RidesInvitations rideinv)
        {

        }


        private async Task GetRideInvitaions()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/RidesInvitations/GetRidesInvitationsByRideId/" + RideId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<RidesInvitations>>(content);
            RequestsList = new List<RidesInvitations>(list);

            if (RequestsList == null || RequestsList.Count == 0)
                IsInvitationsListEmpty = true;

            Busy = false;
        }
    }
}
