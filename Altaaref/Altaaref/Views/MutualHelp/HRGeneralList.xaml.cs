using Altaaref.ViewModels;
using Altaaref.ViewModels.HelpRequests;
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
	public partial class HRGeneralList : ContentPage
	{
		public HRGeneralList ()
		{
			InitializeComponent ();

            BindingContext = new HRGeneralListViewModel(new PageService());
        }
	}
}