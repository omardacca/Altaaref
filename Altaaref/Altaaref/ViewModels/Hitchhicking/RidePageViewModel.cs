using Altaaref.Models;
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

        private bool _isAttendantsEmpty;
        public bool IsAttendantsEmpty
        {
            get { return _isAttendantsEmpty; }
            set
            {
                SetValue(ref _isAttendantsEmpty, value);
            }
        }

        public ICommand ViewProfileCommand => new Command(async () => await HandleViewProfile());

        public RidePageViewModel(Ride ride, IPageService pageService)
        {
            _pageService = pageService;
            Ride = ride;

            Ride.NumOfFreeSeats -= byte.Parse(Ride.RideAttendants.Count.ToString());

            if (ride.RideAttendants.Count > 0)
                IsAttendantsEmpty = false;
            else
                IsAttendantsEmpty = true;
        }

        private async Task HandleViewProfile()
        {
            
        }
    }
}
