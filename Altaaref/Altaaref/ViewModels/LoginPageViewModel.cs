using Altaaref.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Altaaref.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private class logincred
        {
            public string username;
            public string password;
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get { return _isErrorVisible; }
            set { SetValue(ref _isErrorVisible, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetValue(ref _errorMessage, value); }
        }

        private string _usernameEntry;
        public string UsernameEntry
        {
            get { return _usernameEntry; }
            set { SetValue(ref _usernameEntry, value); }
        }

        private string _passwordEntry;
        public string PasswordEntry
        {
            get { return _passwordEntry; }
            set { SetValue(ref _passwordEntry, value); }
        }



        public ICommand LoginCommand => new Command(async () => await LoginAsync(_usernameEntry, _passwordEntry));

        public LoginPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        private async Task LoginAsync(string username, string password)
        {
            logincred news = new logincred { username = username, password = password };

            var request = new HttpRequestMessage(
                HttpMethod.Post, "https://altaarefapp.azurewebsites.net/api/auth/login");

            request.Content = new StringContent(JsonConvert.SerializeObject(news), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                var resCont = await response.Content.ReadAsStringAsync();

                JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(resCont);

                var accessToken = jwtDynamic.Value<string>("auth_token");
                //var accessTokenExpiration = jwtDynamic.Value<DateTime>("expires_in");

                Settings.Username = username;
                Settings.Password = password;
                Settings.AccessToken = accessToken;
                //Settings.AccessTokenExpiration = accessTokenExpiration;

                var page = new Views.MainMenu.MenuPage().GetMenuPage();
                NavigationPage.SetHasNavigationBar(page, false);

                await _pageService.PushAsync(page);
            }
            else
            {
                var error =  await response.Content.ReadAsStringAsync();

                var deserror = JsonConvert.DeserializeObject(error);

                // extract errors later.. 

                ErrorMessage = "Invalid Username or Password";

                IsErrorVisible = true;
            }
        }
    }
}
