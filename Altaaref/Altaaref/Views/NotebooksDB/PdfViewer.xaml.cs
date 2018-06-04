using Altaaref.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PdfViewer : ContentPage
	{
		public PdfViewer (string url)
		{
			InitializeComponent ();

            var localPath = string.Empty;

            if (Device.RuntimePlatform == Device.Android)
            {
                var dependency = DependencyService.Get<ILocalFileProvider>();

                if (dependency == null)
                {
                    DisplayAlert("Error loading PDF", "Computer says no", "OK");

                    return;
                }

                var fileName = Guid.NewGuid().ToString();

                // Download PDF locally for viewing
                using (var httpClient = new HttpClient())
                {
                    var pdfStream = Task.Run(() => httpClient.GetStreamAsync(url)).Result;

                    localPath =
                        Task.Run(() => dependency.SaveFileToDisk(pdfStream, $"{fileName}.pdf")).Result;
                }

                if (string.IsNullOrWhiteSpace(localPath))
                {
                    DisplayAlert("Error loading PDF", "Computer says no", "OK");

                    return;
                }
            }
            
            if (Device.RuntimePlatform == Device.Android)
                PdfView.Source = $"file:///android_asset/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(localPath)}";
            else
                PdfView.Source = url;

        }
	}
}