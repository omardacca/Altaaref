using Altaaref.UserControls;
using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.MutualHelp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewHelpRequests : ContentPage
	{
		public ViewHelpRequests ()
		{
			InitializeComponent ();

            BindingContext = new ViewHelpRequestsViewModel(new PageService());
        }

        public ViewHelpRequests (bool type)
        {
            InitializeComponent();

            BindingContext = new ViewHelpRequestsViewModel(new PageService(), type);
        }

        void HandleResultClicked(object sender, ItemTappedEventArgs e)
        {
            var rrv = sender as RoundedRectangleView;

            (BindingContext as ViewHelpRequestsViewModel).HandleItemClicked(rrv.CommandParameter as StudentHelpRequest);
        }
    }
}