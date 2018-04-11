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

        void HandleItemClicked(object sender, ItemTappedEventArgs e)
        {
            var rrv = sender as RoundedRectangleView;

            (BindingContext as MyStudyGroupsViewModel).StudyGroupItemClicked(rrv.CommandParameter as Models.StudyGroup);
        }
    }
}