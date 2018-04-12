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
	public partial class ViewStudyGroupInvitations : ContentPage
	{
		public ViewStudyGroupInvitations ()
		{
			InitializeComponent ();

            BindingContext = new ViewStudyGroupInvitationsViewModel(new PageService());
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ViewStudyGroupInvitationsViewModel).ViewInvitationSelected(e.Item as ViewInvitation);
        }
    }
}