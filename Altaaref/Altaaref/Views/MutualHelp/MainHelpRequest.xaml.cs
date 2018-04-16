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
	public partial class MainHelpRequest : ContentPage
	{
		public MainHelpRequest ()
		{
			InitializeComponent ();

            BindingContext = new MainHelpRequestViewModel(new PageService());
        }

        void HandleResultClicked(object sender, ItemTappedEventArgs e)
        {
            var rrv = sender as RoundedRectangleView;

            (BindingContext as MainHelpRequestViewModel).HandleResultClicked(rrv.CommandParameter as StudentHelpRequest);
        }
    }
}