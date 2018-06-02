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
    public partial class FindRide : ContentPage
    {
        public FindRide()
        {
            InitializeComponent();

            BindingContext = new FindRideViewModel(new PageService());
            
            search_from.PlacesRetrieved += FromSearchPlacesRetrieved;
            search_from.TextChanged += Search_Bar_TextChanged;
            search_from.MinimumSearchText = 2;

            search_to.PlacesRetrieved += ToSearchPlacesRetrieved;
            search_to.TextChanged += Search_Bar_TextChanged;
            search_to.MinimumSearchText = 2;
        }

        public void FindButtonFocused(object sender, TappedEventArgs result)
        {
            scrollview.ScrollToAsync(findbutton, ScrollToPosition.Start, true);
        }

        void FromSearchPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            (BindingContext as FindRideViewModel).SearchBarPlacesRetrieved(result, "from");
        }

        void ToSearchPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            (BindingContext as FindRideViewModel).SearchBarPlacesRetrieved(result, "to");
        }

        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {


        }
    }
}