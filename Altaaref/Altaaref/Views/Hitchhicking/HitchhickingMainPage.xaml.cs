using Altaaref.ViewModels;
using Altaaref.ViewModels.Hitchhicking;
using DurianCode.PlacesSearchBar;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.Hitchhicking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HitchhickingMainPage : ContentPage
    {

        public HitchhickingMainPage()
        {
            InitializeComponent();

            BindingContext = new HitchhickingMainPageViewModel(new PageService());
        }
    }
}