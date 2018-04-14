using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyHelpRequests : ContentPage
	{
		public MyHelpRequests ()
		{
			InitializeComponent ();

            BindingContext = new MyHelpRequestsViewModel(new PageService());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}