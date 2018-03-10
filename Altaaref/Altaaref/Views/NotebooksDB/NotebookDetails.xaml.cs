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
        public NotebookDetails(int notebookId)
		{
			InitializeComponent ();

            BindingContext = new NotebookDetailsViewModel(notebookId);
        }

        public void OnDownloadButtonClicked(object sender, EventArgs e)
        {
            string url = "https://drive.google.com/uc?authuser=0&id=1qwwKXUOHb2KbJn1pPMsX3xOSA64OIW1N&export=download";

            DependencyService.Get<IDownloader>().StartDownload(url, "pdfToDownload.pdf");
        }

    }
}