using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Altaaref.Helpers;
using Altaaref.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class NotebookDetailsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private bool _isFavorite;

        private string _favoriteImage;
        public string FavoriteImage
        {
            get { return _favoriteImage; }
            set { SetValue(ref _favoriteImage, value); }
        }

        private bool _isOneStars;
        public bool IsOneStars
        {
            get { return _isOneStars; }
            set
            {
                SetValue(ref _isOneStars, value);
            }
        }

        private bool _istwoStars;
        public bool IsTwoStars
        {
            get { return _istwoStars; }
            set
            {
                SetValue(ref _istwoStars, value);
            }
        }

        private bool _isthreeStars;
        public bool IsThreeStars
        {
            get { return _isthreeStars; }
            set
            {
                SetValue(ref _isthreeStars, value);
            }
        }

        private bool _isfourStars;
        public bool IsFourStars
        {
            get { return _isfourStars; }
            set
            {
                SetValue(ref _isfourStars, value);
            }
        }

        private bool _isFiveStars;
        public bool IsFiveStars
        {
            get { return _isFiveStars; }
            set
            {
                SetValue(ref _isFiveStars, value);
            }
        }

        public ICommand FavoriteImageButtonCommand => new Command(OnFavoriteTap);
        public ICommand DownloadCommand => new Command(HandleOnDownloadButtonClicked);
        public ICommand SendToCommand => new Command(HandleSaveToCommand);

        public ICommand ViewProfileCommand { get; set; }
        public ICommand OneStarCommand { get; set; }
        public ICommand TwoStarCommand { get; set; }
        public ICommand ThreeStarCommand { get; set; }
        public ICommand FourStarCommand { get; set; }
        public ICommand FiveStarCommand { get; set; }

        private Dictionary<int,int> _ratesDictionary;
        public Dictionary<int, int> RatesDictionary
        {
            get { return _ratesDictionary; }
            set
            {
                _ratesDictionary = value;
                OnPropertyChanged(nameof(RatesDictionary));
            }
        }


        private NotebookRates _notebookRate;
        public NotebookRates NotebookRate
        {
            get { return _notebookRate; }
            set
            {
                _notebookRate = value;
                OnPropertyChanged(nameof(NotebookRates));
            }
        }

        private ViewNotebookStudent _viewNotebookStudent;
        public ViewNotebookStudent ViewNotebookStudent
        {
            get { return _viewNotebookStudent; }
            set
            {
                _viewNotebookStudent = value;
                OnPropertyChanged(nameof(ViewNotebookStudent));
            }
        }

        private StudentInfoForNotebooks _studentInfo;
        public StudentInfoForNotebooks StudentInfo
        {
            get { return _studentInfo; }
            private set
            {
                _studentInfo = value;
                OnPropertyChanged(nameof(StudentInfo));
            }
        }

        public string CourseName { get; set; }

        private int _notebookFavoritesNumber;
        public int NotebookFavoritesNumber
        {
            get { return _notebookFavoritesNumber; }
            set { SetValue(ref _notebookFavoritesNumber, value); }
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
        
        public NotebookDetailsViewModel(ViewNotebookStudent Notebook)
        {
            Busy = true;
            IsOneStars = false;
            IsTwoStars = false;
            IsThreeStars = false;
            IsFourStars = false;
            IsFiveStars = false;

            OneStarCommand = new Command(OnOneStarTapped);
            TwoStarCommand = new Command(OnTwoStarTapped);
            ThreeStarCommand = new Command(OnThreeStarTapped);
            FourStarCommand = new Command(OnFourStarTapped);
            FiveStarCommand = new Command(OnFiveStarTapped);

            ViewNotebookStudent = Notebook;

            RatesDictionary = new Dictionary<int, int>();

            Task init = InitProperties();
        }

        private async Task InitProperties()
        {
            // check if viewed before, then determine to execute AddViewToViewCount();
            await AddViewToViewCount();

            await GetStudentInfo();

            await GetCourseName();

            await GetNotebookFavoriteNumber();

            await GetNotebookRate();

            await GetRatesDictionary();

            await InitFavoriteImageButton();

            ViewProfileCommand = new Command(OnViewProfileTapped);
            
        }

        #region Rating Area

        #region Tap Rating Stars

        private async void OnFiveStarTapped()
        {
            if(IsFiveStars)
            {
                if (NotebookRate == null)
                    await PostRate(5);
                else
                {
                    NotebookRate.Rate = 5;
                    await PutRate();
                    UpdateToFiveStars();
                }

                UpdateToFiveStars();
            }
            else
            {
                await DeleteRate();
                ResetStars();
            }
        }

        private async void OnFourStarTapped()
        {
            if(IsFourStars)
            {
                if (NotebookRate == null)
                    await PostRate(4);
                else
                {
                    NotebookRate.Rate = 4;
                    await PutRate();
                    UpdateToFourStars();
                }
                UpdateToFourStars();
            }
            else
            {
                if(!IsFourStars && IsFiveStars)
                {
                    NotebookRate.Rate = 4;
                    await PutRate();
                    UpdateToFourStars();
                }
                else
                {
                    await DeleteRate();
                    ResetStars();
                }
            }
        }
        
        private async void OnThreeStarTapped()
        {
            if (IsThreeStars)
            {
                if (NotebookRate == null)
                    await PostRate(3);
                else
                {
                    NotebookRate.Rate = 3;
                    await PutRate();
                    UpdateToThreeStars();
                }
                UpdateToThreeStars();
            }
            else
            {
                if (!IsThreeStars && (IsFourStars || IsFiveStars))
                {
                    NotebookRate.Rate = 3;
                    await PutRate();
                    UpdateToThreeStars();
                }
                else
                {
                    await DeleteRate();
                    ResetStars();
                }
            }
        }

        private async void OnTwoStarTapped()
        {
            if(IsTwoStars)
            {
                if (NotebookRate == null)
                    await PostRate(2);
                else
                {
                    NotebookRate.Rate = 2;
                    await PutRate();
                    UpdateToTwoStars();
                }
                UpdateToTwoStars();

            }
            else
            {
                if(!IsTwoStars && (IsThreeStars || IsFourStars || IsFiveStars))
                {
                    NotebookRate.Rate = 2;
                    await PutRate();
                    UpdateToTwoStars();
                }
                else
                {
                    await DeleteRate();
                    ResetStars();
                }
            }
        }

        private async void OnOneStarTapped()
        {
            if (IsOneStars)
            {
                if (NotebookRate == null)
                    await PostRate(1);

                UpdateToOneStars();
            }
            else
            {
                if (!IsOneStars && (IsTwoStars || IsThreeStars || IsFourStars || IsFiveStars))
                {
                    NotebookRate.Rate = 1;
                    await PutRate();
                    UpdateToOneStars();
                }
                else
                {
                    await DeleteRate();
                    ResetStars();
                }
            }
        }

        private void ResetStars()
        {
            IsOneStars = false;
            IsTwoStars = false;
            IsThreeStars = false;
            IsFourStars = false;
            IsFiveStars = false;
        }

        private void UpdateToTwoStars()
        {
            IsFiveStars = false;
            IsFourStars = false;
            IsThreeStars = false;
            IsOneStars = true;
            IsTwoStars = true;
        }

        private void UpdateToThreeStars()
        {
            IsFiveStars = false;
            IsFourStars = false;
            IsTwoStars = true;
            IsOneStars = true;
            IsThreeStars = true;
        }

        private void UpdateToFourStars()
        {
            IsFiveStars = false;
            IsThreeStars = true;
            IsTwoStars = true;
            IsOneStars = true;
            IsFourStars = true;
        }

        private void UpdateToFiveStars()
        {
            IsOneStars = true;
            IsTwoStars = true;
            IsThreeStars = true;
            IsFourStars = true;
            IsFiveStars = true;
        }

        private void UpdateToOneStars()
        {
            IsFiveStars = false;
            IsFourStars = false;
            IsThreeStars = false;
            IsTwoStars = false;
            IsOneStars = true;
        }

        #endregion

        public async Task PutRate()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/" + ViewNotebookStudent.Notebook.Id + "/" + Settings.Identity;

            var content = new StringContent(JsonConvert.SerializeObject(NotebookRate), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, content);

            Busy = false;
        }

        public async Task DeleteRate()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/" + ViewNotebookStudent.Notebook.Id + "/" + Settings.Identity;

            try
            {
                var content = await _client.DeleteAsync(url);

                Busy = false;
            }
            catch(HttpRequestException e)
            {

                Busy = false;
            }

            Busy = false;
        }

        public async Task PostRate(byte rate)
        {
            Busy = true;

            NotebookRates sfn = new NotebookRates { StudentId = Settings.StudentId, NotebookId = _viewNotebookStudent.Notebook.Id, Rate = rate };

            var content = new StringContent(JsonConvert.SerializeObject(sfn), Encoding.UTF8, "application/json");

            _client.BaseAddress = new Uri("https://altaarefapp.azurewebsites.net");
            var response = await _client.PostAsync("api/NotebookRates", content);
            //return response.IsSuccessStatusCode;

            Busy = false;
        }

        public async Task GetNotebookRate()
        {
 
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/" + ViewNotebookStudent.Notebook.Id + "/" + Settings.Identity;
            try
            {
                var content = await _client.GetStringAsync(url);
                var nr = JsonConvert.DeserializeObject<NotebookRates>(content);
                NotebookRate = nr;

                updateStars(NotebookRate.Rate);

                Busy = false;
            }
            catch(HttpRequestException e)
            {
                NotebookRate = null;
                Busy = false;
            }

            Busy = false;
        }

        private void updateStars(int rate)
        {
            switch(rate)
            {
                case 1: UpdateToOneStars();
                    break;
                case 2: UpdateToTwoStars();
                    break;
                case 3: UpdateToThreeStars();
                    break;
                case 4: UpdateToFourStars();
                    break;
                case 5: UpdateToFiveStars();
                    break;
            }
        }

        #endregion

        public async Task GetRatesDictionary()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/GetAllNotebookRates/" + ViewNotebookStudent.Notebook.Id;

            string content = await _client.GetStringAsync(url);
            var dic = JsonConvert.DeserializeObject<Dictionary<int,int>>(content);
            RatesDictionary = dic;
            Busy = false;
        }

        public async Task GetNotebookFavoriteNumber()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Notebooks/StudentFavoriteNumber/" + ViewNotebookStudent.Notebook.Id;

            var content = await _client.GetStringAsync(url);

            NotebookFavoritesNumber = int.Parse(content);

            Busy = false;
        }

        public async Task GetCourseName()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Courses/CourseName/" + ViewNotebookStudent.Notebook.CourseId;

            string content = await _client.GetStringAsync(url);
            var obj = JsonConvert.DeserializeObject(content);

            CourseName = obj.ToString();

            Busy = false;
        }

        public async Task GetStudentInfo()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/Students/Infofornotebooks/" + Settings.Identity

            string content = await _client.GetStringAsync(url);
            var obj = JsonConvert.DeserializeObject<StudentInfoForNotebooks>(content);
            StudentInfo = obj;

            Busy = false;
        }

        private void OnViewProfileTapped()
        {
            // push async to profile..
        }

        // Get if current is favorite or not
        private async Task UpdateFieldFavoriteStatus()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/" + Settings.Identity + "/" + _viewNotebookStudent.Notebook.Id;

            var content = await _client.GetAsync(url);
            var response = content.IsSuccessStatusCode;

            if (response)
                _isFavorite = true;
            else
                _isFavorite = false;
        }

        // Add Current to favorite
        private bool AddToFavorite()
        {
            StudentFavNotebooks sfn = new StudentFavNotebooks { StudentId = Settings.StudentId, NotebookId = _viewNotebookStudent.Notebook.Id };

            var content = new StringContent(JsonConvert.SerializeObject(sfn), Encoding.UTF8, "application/json");

            _client.BaseAddress = new Uri("https://altaarefapp.azurewebsites.net");
            var response = _client.PostAsync("api/StudentFavNotebooks", content).Result;
            return response.IsSuccessStatusCode;
        }

        // Deletes current Favorite
        private bool DeleteFavorite()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/" + Settings.Identity + "/" + _viewNotebookStudent.Notebook.Id;
            var response = _client.DeleteAsync(url);
            return response.Result.IsSuccessStatusCode;
        }

        private async Task InitFavoriteImageButton()
        {
            await UpdateFieldFavoriteStatus();
            if (_isFavorite)
            {
                _isFavorite = true;
                FavoriteImage = "favoriteon.png";
            }
            else
            {
                _isFavorite = false;
                FavoriteImage = "favorite.png";
            }
        }

        private void OnFavoriteTap()
        {
            if(_isFavorite) // was favorite, make it not favorite
            {
                DeleteFavorite();
                _isFavorite = false;
                FavoriteImage = "favorite.png";
            }
            else
            {
                AddToFavorite();
                _isFavorite = true;
                FavoriteImage = "favoriteon.png";
            }
        }

        private async Task GetNotebookAsync()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/" + _viewNotebookStudent.Notebook.Id;
            string content = await _client.GetStringAsync(url);
            ViewNotebookStudent.Notebook = JsonConvert.DeserializeObject<Notebook>(content);

            Busy = false;
        }

        private async Task AddViewToViewCount()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/" + _viewNotebookStudent.Notebook.Id;

            _viewNotebookStudent.Notebook.ViewsCount += 1;
            
            var content = new StringContent(JsonConvert.SerializeObject(_viewNotebookStudent.Notebook), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, content);

            // update the Notebook property from db
            await GetNotebookAsync();
        }

        private void HandleOnDownloadButtonClicked()
        {
            DependencyService.Get<IDownloader>().StartDownload(_viewNotebookStudent.Notebook.BlobURL, _viewNotebookStudent.Notebook.Name.Trim() + ".pdf");
        }

        private void HandleSaveToCommand()
        {
            DependencyService.Get<IDownloader>().SaveTo();
        }
        
    }
}
