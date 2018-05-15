using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IPageService _pageService;

        private string _emailEntry;
        public string EmailEntry
        {
            get { return _emailEntry; }
            set { SetValue(ref _emailEntry, value); }
        }

        private string _passwordEntry;
        public string PasswordEntry
        {
            get { return _passwordEntry; }
            set { SetValue(ref _passwordEntry, value); }
        }

        private string _verifyPasswordEntry;
        public string VerifyPasswordEntry
        {
            get { return _verifyPasswordEntry; }
            set { SetValue(ref _verifyPasswordEntry, value); }
        }

        private string _fullNameEntry;
        public string FullNameEntry
        {
            get { return _fullNameEntry; }
            set { SetValue(ref _fullNameEntry, value); }
        }

        private int _studentIdEntry;
        public int StudentIdEntry
        {
            get { return _studentIdEntry; }
            set { SetValue(ref _studentIdEntry, value); }
        }

        private DateTime _dobDatePicker;
        public DateTime DOBDatePicker
        {
            get { return _dobDatePicker; }
            set { SetValue(ref _dobDatePicker, value); }
        }

        public ICommand RegisterCommand => new Command(async () => await HandleRegisterTap());
        public ICommand LoginPageCommand => new Command(async () => await HandleLoginTap());

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

        public RegisterViewModel(IPageService pageService)
        {
            _pageService = pageService;
            DOBDatePicker = DateTime.Now;
        }

        private async Task HandleLoginTap()
        {
            await _pageService.PushAsync(new Views.LoginPage());
        }

        private async Task HandleRegisterTap()
        {
            bool isSuccess = await PostRegister();

            if(isSuccess)
            {
                var page = new Views.MainMenu.MenuPage().GetMenuPage();
                NavigationPage.SetHasNavigationBar(page, false);
                await _pageService.PushAsync(page);
            }
        }

        private async Task<bool> PostRegister()
        {
            HttpClient _client = new HttpClient();

            var request = new HttpRequestMessage(
                HttpMethod.Post, "https://altaarefapp.azurewebsites.net/api/Accounts");

            Student std = new Student
            {
                IdentityId = Settings.Identity,
                Id = _studentIdEntry,
                FullName = _fullNameEntry,
                DOB = _dobDatePicker,
                ProfilePicBlobUrl = "https://csb08eb270fff55x4a98xb1a.blob.core.windows.net/profilepics/IMG_8038.JPG"
            };

            var serStd = JsonConvert.SerializeObject(std);

            request.Content = new StringContent(serStd, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var resCont = await response.Content.ReadAsStringAsync();
                JObject resObj = JsonConvert.DeserializeObject<dynamic>(resCont);

                var error = resObj.Value<string>("DuplicateUserName");

                ErrorMessage = error;
                IsErrorVisible = true;

                return false;
            }
        }

        private bool IsFormValid()
        {
            if (_passwordEntry != _verifyPasswordEntry) return false;
            if (_studentIdEntry.ToString().Length != 9) return false;
            // ..

            return true;
        }
    }
}
