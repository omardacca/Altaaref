using Altaaref.Models;
using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FacultiesListPage : ContentPage
	{
        public FacultiesListPage ()
		{
            BindingContext = new FacultiesListViewModel();

            InitializeComponent ();
        }

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (BindingContext as FacultiesListViewModel).FacultySelected(e.SelectedItem as Faculty );

            await this.Navigation.PushAsync(new Views.NotebooksDB.FacultyCoursesListPage());

            //Deselect Item
            // I Used OnPropertyChanged within BaseViewModel Class, but something wrong happening
            ((ListView)sender).SelectedItem = null;
        }
    }
}