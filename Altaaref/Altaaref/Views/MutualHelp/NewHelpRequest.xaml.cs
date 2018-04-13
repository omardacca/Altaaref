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
	public partial class NewHelpRequest : ContentPage
	{
		public NewHelpRequest ()
		{
			InitializeComponent ();

            BindingContext = new NewHelpRequestViewModel(new PageService());
        }
	}
}