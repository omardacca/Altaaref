using Altaaref.Helpers;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
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
        
        public ICommand GetLoc => new Command(async () => await GetLocation());
        public ICommand AddRideCommand => new Command(async () => await NavigateToAddRide());
        public ICommand FindRideCommand => new Command(async () => await NavigateToFindRide());


        public HitchhickingMainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
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
