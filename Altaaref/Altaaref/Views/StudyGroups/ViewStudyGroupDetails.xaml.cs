using Altaaref.UserControls;
using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.StudyGroups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewStudyGroupDetails : ContentPage
	{
		public ViewStudyGroupDetails (Models.StudyGroup studyGroup)
		{
			InitializeComponent ();

            BindingContext = new ViewStudyGroupDetailsViewModel(studyGroup, new PageService());
        }

        void HandleJoinClicked(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ViewStudyGroupDetailsViewModel).HandlePostAttendant();
        }

        void HandleLeaveClicked(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ViewStudyGroupDetailsViewModel).HandleRemoveAttendant();
        }

        void HandleViewAttendants(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ViewStudyGroupDetailsViewModel).HandleViewAttendants();
        }



    }
}