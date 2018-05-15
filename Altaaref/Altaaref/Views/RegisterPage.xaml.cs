using Altaaref.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();

            BindingContext = new RegisterViewModel(new PageService());
        }
	}
}