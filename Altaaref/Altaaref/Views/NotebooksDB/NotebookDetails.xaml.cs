using Altaaref.Models;
using Altaaref.ViewModels;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotebookDetails : ContentPage
	{
        public NotebookDetails(ViewNotebookStudent notebook)
		{
            BindingContext = new NotebookDetailsViewModel(notebook);

            InitializeComponent ();
        }

        /*
        public void OnDownloadButtonClicked(object sender, EventArgs e)
        {
            //           string url = "https://drive.google.com/uc?authuser=0&id=1qwwKXUOHb2KbJn1pPMsX3xOSA64OIW1N&export=download";

            //            DependencyService.Get<IDownloader>().StartDownload(url, "pdfToDownload.pdf");

            (BindingContext as NotebookDetailsViewModel).HandleOnDownloadButtonClicked();
        }

        // temporarly Implemented this button to download file from external url, as Stream, In order to
        // upload it as blob to azure (in ViewModel)
        public void OnFavoriteButtonClicked(object sender, EventArgs e)
        {
            //string url = "https://drive.google.com/uc?authuser=0&id=1qwwKXUOHb2KbJn1pPMsX3xOSA64OIW1N&export=download";

            string url1 = "http://www2.padi.com/blog/wp-content/uploads/2017/04/shutterstock_366792137-1024x683.jpg";

            (BindingContext as NotebookDetailsViewModel).HandleFavoriteButtonClicked(url1);
        }

        
        public void OnLocationButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<ILocationProvider>().getCurrentLocation();
        }
        */
    }
}