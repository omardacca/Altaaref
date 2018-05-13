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
            InitializeComponent ();

            BindingContext = new NotebookDetailsViewModel(notebook);
        }
    }
}