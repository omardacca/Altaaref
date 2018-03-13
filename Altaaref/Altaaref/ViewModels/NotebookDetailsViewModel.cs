﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        private Notebook _notebook;
        public Notebook Notebook
        {
            get { return _notebook; }
            set
            {
                _notebook = value;
                OnPropertyChanged(nameof(Notebook));
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

        public NotebookDetailsViewModel(int notebookId)
        {
            // Enable activity Indicator - disable is right after assigning listView item source
            Busy = true;

            GetNotebookAsync(notebookId);
        }

        private async void GetNotebookAsync(int notebookId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Notebooks/" + notebookId;

            string content = await _client.GetStringAsync(url);
            Notebook = JsonConvert.DeserializeObject<Notebook>(content);

            // Disable Activity Idicator
            Busy = false;
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
            DependencyService.Get<IDownloader>().StartDownload(Notebook.BlobURL, Notebook.FileName);
        }


        // same note as UploadFileToBlob method up there,
        // Favorite button implementation should be changed obviously
        public void HandleFavoriteButtonClicked(string url)
        {
            //await UploadFileToBlob(await getStreamAsync(url));
            PostFavorite();
        }

        public void PostFavorite()
        {
            // Currently used StudentId = 204228043
            StudentFavNotebooks sfn = new StudentFavNotebooks { StudentId = 204228043, NotebookId = Notebook.Id };

            var content = new StringContent(JsonConvert.SerializeObject(sfn));

            _client.BaseAddress = new Uri("https://altaarefapp.azurewebsites.net");
            var response = _client.PostAsync("https://altaarefapp.azurewebsites.net", content).Result;
            if (response.IsSuccessStatusCode)
            {
                //Console.Write("Success");
            }
        }
    }
}
