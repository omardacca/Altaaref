using Altaaref.Helpers;
using Altaaref.ViewModels;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public static int LOGOUT_CODE = 1;
        public static int LOGIN_CODE = 2;

		public LoginPage ()
		{
			InitializeComponent ();

            var loggedin = IsLoggedIn();

            BindingContext = new LoginPageViewModel(new PageService());
        }

        public LoginPage(int sourcePage)
        {
            if(sourcePage == LOGOUT_CODE)
            {
                Settings.AccessToken = "";
                Settings.Username = "";
                Settings.Password = "";
                Settings.Identity = "";
                Settings.StudentId = 0;
            }

            InitializeComponent();
            BindingContext = new LoginPageViewModel(new PageService());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var loggedin = await IsLoggedIn();
        }

        private async Task<bool> IsLoggedIn()
        {
            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                var page = new Views.MainMenu.MenuPage().GetMenuPage();
                NavigationPage.SetHasNavigationBar(page, false);

                await (new PageService()).PushAsync(page);
                return true;
            }
            return false;
        }
	}
}