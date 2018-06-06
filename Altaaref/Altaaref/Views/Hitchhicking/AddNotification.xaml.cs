using Altaaref.ViewModels;
using Altaaref.ViewModels.Hitchhicking;
using DurianCode.PlacesSearchBar;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.Hitchhicking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddNotification : ContentPage
	{
		public AddNotification ()
		{
			InitializeComponent ();

            BindingContext = new ViewModels.Hitchhicking.AddNotificationViewModel(new PageService());

            search_from.PlacesRetrieved += FromSearchPlacesRetrieved;
            search_from.TextChanged += Search_Bar_TextChanged;
            search_from.MinimumSearchText = 2;

            search_to.PlacesRetrieved += ToSearchPlacesRetrieved;
            search_to.TextChanged += Search_Bar_TextChanged;
            search_to.MinimumSearchText = 2;
        }


        void FromSearchPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            (BindingContext as AddNotificationViewModel).SearchBarPlacesRetrieved(result, "from");
        }

        void ToSearchPlacesRetrieved(object sender, AutoCompleteResult result)
        {
            (BindingContext as AddNotificationViewModel).SearchBarPlacesRetrieved(result, "to");
        }

        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {


        }
    }
}