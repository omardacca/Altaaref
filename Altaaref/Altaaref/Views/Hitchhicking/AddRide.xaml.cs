using Altaaref.ViewModels;
using Altaaref.ViewModels.Hitchhicking;
using DurianCode.PlacesSearchBar;
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
	public partial class AddRide : ContentPage
	{
		public AddRide ()
		{
			InitializeComponent ();

            BindingContext = new AddRideViewModel(new PageService());

            search_from.PlacesRetrieved += FromSearchPlacesRetrieved;
            search_from.TextChanged += Search_Bar_TextChanged;
            search_from.MinimumSearchText = 2;

            search_to.PlacesRetrieved += ToSearchPlacesRetrieved;
            search_to.TextChanged += Search_Bar_TextChanged;
            search_to.MinimumSearchText = 2;
        }


        void FromSearchPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            (BindingContext as AddRideViewModel).SearchBarPlacesRetrieved(result, "from");
        }

        void ToSearchPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            (BindingContext as AddRideViewModel).SearchBarPlacesRetrieved(result, "to");
        }

        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

    }
}