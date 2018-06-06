using Altaaref.ViewModels;
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
	public partial class AddNewNotebook : ContentPage
	{
		public AddNewNotebook ()
		{
			InitializeComponent ();

            BindingContext = new AddNewNotebookViewModel(new PageService());

        }

        public void IndexChangedEvent()
        {
            (BindingContext as AddNewNotebookViewModel).PickerItemSelectedFlag = true;
        }

    }
}