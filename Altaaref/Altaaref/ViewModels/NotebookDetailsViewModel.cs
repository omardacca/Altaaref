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


        public ICommand FavoriteImageButtonCommand { get; private set; }

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

            ViewNotebookStudent = Notebook;

            InitProperties();
        }

        private void InitProperties()
        {
            // check if viewed before, then determine to execute AddViewToViewCount();
            AddViewToViewCount();

            InitFavoriteImageButton();

            FavoriteImageButtonCommand = new Command(OnFavoriteTap);
        }

        // Get if current is favorite or not
        private void UpdateFieldFavoriteStatus()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFavNotebooks/" + _tempStudentId + "/" + _viewNotebookStudent.Notebook.Id;

            var content = _client.GetAsync(url).Result;
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

        private void InitFavoriteImageButton()
        {
            UpdateFieldFavoriteStatus();
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


        private async void AddViewToViewCount()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/" + _viewNotebookStudent.Notebook.Id;

            Notebook updated = _viewNotebookStudent.Notebook;
            updated.ViewsCount += 1;

            var content = new StringContent(JsonConvert.SerializeObject(updated), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(url, content).Result;

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

        public void HandleOnDownloadButtonClicked()
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
