using Altaaref.ViewModels;
using Altaaref.ViewModels.Notebooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindNotebookResults : ContentPage
	{
		public FindNotebookResults (bool SwitchStatues, int? CourseId=null, string Notebooknametext = null)
		{
			InitializeComponent ();

            BindingContext = new FindNotebookResultsViewModel(new PageService(), SwitchStatues, CourseId, Notebooknametext);
        }
	}
}