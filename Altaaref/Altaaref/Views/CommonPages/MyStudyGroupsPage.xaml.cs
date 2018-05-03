using Altaaref.UserControls;
using Altaaref.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyStudyGroupsPage : ContentPage
	{
		public MyStudyGroupsPage ()
		{
			InitializeComponent ();

            BindingContext = new MyStudyGroupsViewModel(new PageService());
        }
    }
}