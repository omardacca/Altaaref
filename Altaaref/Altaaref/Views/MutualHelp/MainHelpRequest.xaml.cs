using Altaaref.UserControls;
using Altaaref.ViewModels;
using Altaaref.ViewModels.HelpRequest;
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

            BindingContext = new MainPageHelpRequestViewModel(new PageService());
        }

    }
}