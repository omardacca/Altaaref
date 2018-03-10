using Altaaref.Models;
using Altaaref.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FacultiesListPage : ContentPage
	{
        public FacultiesListPage()
		{
            BindingContext = new FacultiesListViewModel(new PageService());

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await (BindingContext as FacultiesListViewModel).FacultySelectedAsync(e.Item as Faculty);
        }
    }
}