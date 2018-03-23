using Altaaref.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class AddNewNotebookViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private List<Courses> _coursesList;
        public List<Courses> CoursesList
        {
            get { return _coursesList; }
            private set
            {
                _coursesList = value;
                OnPropertyChanged(nameof(CoursesList));
            }
        }

        private List<string> _coursesNamesList;
        public List<string> CoursesNameList
        {
            get { return _coursesNamesList; }
            set
            {
                _coursesNamesList = value;
                OnPropertyChanged(nameof(CoursesNameList));
            }
        }

        private string _titleEntry;
        public string TitleEntry
        {
            get { return _titleEntry; }
            set { SetValue(ref _titleEntry, value); }
        }

        private string _urlEntry;
        public string UlrEnty
        {
            get { return _urlEntry; }
            set { SetValue(ref _urlEntry, value); }
        }

        public ICommand HandleSubmition { get; private set; }

        private int _selectedCourseIndex;
        public int SelectedCourseIndex
        {
            get { return _selectedCourseIndex; }
            set { SetValue(ref _selectedCourseIndex, value); }
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

        private readonly IPageService _pageService;

        public AddNewNotebookViewModel(IPageService pageService)
        {
            _pageService = pageService;
            CoursesNameList = new List<string>();
            HandleSubmition = new Command(OnSubmitButtonTapped);

            Init();
        }

        private async void OnSubmitButtonTapped()
        {
            var UploadedUri = await UploadFileFromUrl();
            var coursename = _coursesList[_selectedCourseIndex].Id;
            Notebook newNotebook = new Notebook()
            {
                Name = this.TitleEntry,
                FileName = this.TitleEntry.Trim(), // will delete this column later
                BlobURL = UploadedUri,
                CourseId = coursename
            };

            var postUrl = "https://altaarefapp.azurewebsites.net/api/Notebooks";

            var content = new StringContent(JsonConvert.SerializeObject(newNotebook), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content).Result;

            if(response.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Verification", "We will check and verify your notebook as soon as possible, thanks for sharing.", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
            }
        }

        private async Task<string> UploadFileFromUrl()
        {
            return await UploadFileToBlob(await GetStreamAsync(UlrEnty), TitleEntry.Trim() + Guid.NewGuid() + ".pdf");
        }

        private Task<Stream> GetStreamAsync(string url)
        {
            var httpClient = new HttpClient();
            return httpClient.GetStreamAsync(new Uri(url));
        }

        public async Task<string> UploadFileToBlob(Stream fileStream, string filename)
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
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            // Create the "filename" blob with the text "Hello, world!"
            await blockBlob.UploadFromStreamAsync(fileStream);

            return blockBlob.Uri.AbsoluteUri;
        }

        async void Init()
        {
            Busy = true;
            await GetCoursesAsync();
            InitCoursesList();
            Busy = false;
        }

        private void InitCoursesList()
        {
            foreach (var c in CoursesList)
                CoursesNameList.Add(c.Name);
        }

        private async Task GetCoursesAsync()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Courses";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            CoursesList = new List<Courses>(list);
        }

        

    }
}
