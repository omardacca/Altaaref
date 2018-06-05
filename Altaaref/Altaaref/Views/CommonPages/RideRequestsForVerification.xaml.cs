using Altaaref.Models;
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
	public partial class RideRequestsForVerification : ContentPage
	{
		public RideRequestsForVerification (Ride Ride)
		{
			InitializeComponent ();

            BindingContext = new ViewModels.Hitchhicking.RideRequestsForVerificationViewModel(new PageService(), Ride);
		}
	}
}