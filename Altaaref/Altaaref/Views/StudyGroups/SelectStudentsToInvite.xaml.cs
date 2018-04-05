using Altaaref.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.StudyGroups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectStudentsToInvite : ContentPage
	{
		public SelectStudentsToInvite (int courseId, int studyGroupId)
		{
			InitializeComponent ();

            BindingContext = new SelectStudentsToInviteViewModel(new PageService(), courseId, studyGroupId);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as SelectStudentsToInviteViewModel).StudentSelectedAsync(e.Item as ViewStudent);
        }
	}
}