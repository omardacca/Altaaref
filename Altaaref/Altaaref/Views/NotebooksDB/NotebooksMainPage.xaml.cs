using Altaaref.ViewModels;
using Altaaref.ViewModels.HelpRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotebooksMainPage : ContentPage
	{
		public NotebooksMainPage ()
		{
			InitializeComponent ();

            BindingContext = new NotebooksMainPageViewModel(new PageService());
        }
    }
}