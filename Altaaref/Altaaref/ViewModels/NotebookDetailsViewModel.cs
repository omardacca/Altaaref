using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Altaaref.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class NotebookDetailsViewModel : BaseViewModel
    {
        int _tempStudentId = 204228043;
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

        public ICommand FavoriteImageButtonCommand { get; private set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand ViewProfileCommand { get; set; }
        public ICommand OneStarCommand { get; set; }
        public ICommand TwoStarCommand { get; set; }
        public ICommand ThreeStarCommand { get; set; }
        public ICommand FourStarCommand { get; set; }
        public ICommand FiveStarCommand { get; set; }


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

            Task init = InitProperties();
        }

        private async void OnFiveStarTapped()
        {
            if(IsFiveStars)
            {
                if (NotebookRate == null)
                    await PostRate(5);

                IsOneStars = true;
                IsTwoStars = true;
                IsThreeStars = true;
                IsFourStars = true;
            }
            else
            {
                await DeleteRate();
            }
        }

        private async void OnFourStarTapped()
        {
            if(IsFourStars)
            {
                if (NotebookRate == null)
                    await PostRate(4);

                IsFiveStars = false;
                IsThreeStars = true;
                IsTwoStars = true;
                IsOneStars = true;
            }
            else
            {
                await DeleteRate();
                IsFiveStars = false;
            }
        }

        private async void OnThreeStarTapped()
        {
            if(IsThreeStars)
            {
                if (NotebookRate == null)
                    await PostRate(3);

                IsFiveStars = false;
                IsFourStars = false;
                IsTwoStars = true;
                IsOneStars = true;
            }
            else
            {
                await DeleteRate();
                IsFiveStars = false;
                IsFourStars = false;
            }
        }

        private async void OnTwoStarTapped()
        {
            if(IsTwoStars)
            {
                if (NotebookRate == null)
                    await PostRate(2);

                IsFiveStars = false;
                IsFourStars = false;
                IsThreeStars = false;
                IsOneStars = true;
            }
            else
            {
                await DeleteRate();

                IsFiveStars = false;
                IsFourStars = false;
                IsThreeStars = false;
            }
        }

        private async void OnOneStarTapped()
        {
            if(IsOneStars)
            {
                if (NotebookRate == null)
                    await PostRate(1);

                IsFiveStars = false;
                IsFourStars = false;
                IsThreeStars = false;
                IsTwoStars = false;
            }
            else
            {
                await DeleteRate();

                IsFiveStars = false;
                IsFourStars = false;
                IsThreeStars = false;
                IsTwoStars = false;
            }
        }

        private async Task InitProperties()
        {
            // check if viewed before, then determine to execute AddViewToViewCount();
            await AddViewToViewCount();

            await GetStudentInfo();

            await GetCourseName();

            await GetNotebookFavoriteNumber();

            await GetNotebookRate();

            await InitFavoriteImageButton();

            FavoriteImageButtonCommand = new Command(OnFavoriteTap);
            DownloadCommand = new Command(HandleOnDownloadButtonClicked);
            ViewProfileCommand = new Command(OnViewProfileTapped);
            
        }

        public async Task DeleteRate()
        {
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/" + ViewNotebookStudent.Notebook.Id + "/" + ViewNotebookStudent.StudentId;

            try
            {
                var content = await _client.DeleteAsync(url);
            }
            catch(ArgumentNullException e)
            {

            }
        }

        public async Task PostRate(byte rate)
        {
            NotebookRates sfn = new NotebookRates { StudentId = 204228043, NotebookId = _viewNotebookStudent.Notebook.Id, Rate = rate };

            var content = new StringContent(JsonConvert.SerializeObject(sfn), Encoding.UTF8, "application/json");

            _client.BaseAddress = new Uri("https://altaarefapp.azurewebsites.net");
            var response = await _client.PostAsync("api/NotebookRates", content);
            //return response.IsSuccessStatusCode;
        }

        public async Task GetNotebookRate()
        {
 
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/NotebookRates/" + ViewNotebookStudent.Notebook.Id;
            try
            {
                var content = await _client.GetStringAsync(url);
                var nr = JsonConvert.DeserializeObject<NotebookRates>(content);
                NotebookRate = nr;
            }
            catch(ArgumentNullException e)
            {
                NotebookRate = null;
            }

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
            var url = "https://altaarefapp.azurewebsites.net/api/Students/Infofornotebooks/" + ViewNotebookStudent.StudentId;

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
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/" + _tempStudentId + "/" + _viewNotebookStudent.Notebook.Id;

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
            StudentFavNotebooks sfn = new StudentFavNotebooks { StudentId = 204228043, NotebookId = _viewNotebookStudent.Notebook.Id };

            var content = new StringContent(JsonConvert.SerializeObject(sfn), Encoding.UTF8, "application/json");

            _client.BaseAddress = new Uri("https://altaarefapp.azurewebsites.net");
            var response = _client.PostAsync("api/StudentFavNotebooks", content).Result;
            return response.IsSuccessStatusCode;
        }

        // Deletes current Favorite
        private bool DeleteFavorite()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/" + _tempStudentId + "/" + _viewNotebookStudent.Notebook.Id;
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

        // reads and returns external file as Stream
        private  Task<Stream> getStreamAsync(string url)
        {
            var httpClient = new HttpClient(); 
            return httpClient.GetStreamAsync(new Uri(url));
        }

        // This method should not be here, and it should be in 'Adding new Notebook Form..'
        // but for the simplicity and to try the method, I created it here..
        public async Task<bool> UploadFileToBlob(Stream fileStream)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=csb08eb270fff55x4a98xb1a;AccountKey=7ROeIOcZq54z+OnYRzR+YJow+sSu3ElALl/HCxjX/LaGLQy6eDY8Ij/E1aFNC4v1ls0SUHPteDzkU1cBzrPpXw==;EndpointSuffix=core.windows.net");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("notebooks");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            // Retrieve reference to a blob named "filename".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("newImage.jpg");

            // Create the "filename" blob with the text "Hello, world!"
            await blockBlob.UploadFromStreamAsync(fileStream);

            return await Task.FromResult(true);
        }

        private void HandleOnDownloadButtonClicked()
        {
            DependencyService.Get<IDownloader>().StartDownload(_viewNotebookStudent.Notebook.BlobURL, _viewNotebookStudent.Notebook.FileName);
        }

        // same note as UploadFileToBlob method up there,
        // Favorite button implementation should be changed obviously
        public async void HandleFavoriteButtonClicked(string url)
        {
            await UploadFileToBlob(await getStreamAsync(url));
        }

        
    }
}
