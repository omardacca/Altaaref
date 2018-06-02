using Altaaref.Models;
using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.Hitchhicking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RidePage : ContentPage
	{
		public RidePage ()
		{
			InitializeComponent ();
		}

        public RidePage(Ride ride)
        {
            InitializeComponent();

            BindingContext = new ViewModels.Hitchhicking.RidePageViewModel(ride, new PageService());
        }
	}
}