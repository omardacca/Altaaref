using Altaaref.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.StudyGroups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewGroupPage : ContentPage
	{
        public NewGroupPage ()
		{
			InitializeComponent ();

            BindingContext = new NewGroupViewModel(new PageService());
        }
	}
}