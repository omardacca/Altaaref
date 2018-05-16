using Altaaref.Models;
using Altaaref.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectCoursesForRegisterationPage : ContentPage
	{
		public SelectCoursesForRegisterationPage(List<Faculty> FacultiesSelectedList)
		{
			InitializeComponent ();

            BindingContext = new SelectCoursesForRegisterationViewModel(new PageService(), FacultiesSelectedList);
        }
	}
}