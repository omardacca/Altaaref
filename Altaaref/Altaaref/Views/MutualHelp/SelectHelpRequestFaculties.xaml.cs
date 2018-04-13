using Altaaref.Models;
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
	public partial class SelectHelpRequestFaculties : ContentPage
	{
		public SelectHelpRequestFaculties (HelpRequest newHelpRequest)
		{
			InitializeComponent ();

            BindingContext = new SelectHelpRequestFacultiesViewModel(new PageService(), newHelpRequest);
        }
	}
}