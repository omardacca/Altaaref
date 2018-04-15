using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewHelpRequestsDetails : ContentPage
	{
		public ViewHelpRequestsDetails(StudentHelpRequest studentHelpRequest)
		{
			InitializeComponent ();

            BindingContext = new ViewHelpRequestsDetailsViewModel(new PageService(), studentHelpRequest);
        }

        void HandleCommentTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ViewHelpRequestsDetailsViewModel).HanldeCommentTapped(e.Item as StudentHelpComment);
        }
    }
}