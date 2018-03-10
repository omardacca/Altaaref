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
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FacultiesListPage : ContentPage
	{
        public FacultiesListPage ()
		{
            BindingContext = new FacultiesListViewModel();

            InitializeComponent ();
        }

        private HttpClient _client = new HttpClient();
        private ObservableCollection<Faculty> faculty;

        protected override async void OnAppearing()
        {

            string url = "http://localhost:53626/api/faculties";

            /*
             *  Calling Fake API, Works fine BUT Not with Xamarin Live.
                 worked with my physical android machine

                Next: calling my API that is in localhost will not work..
                Solution: run it with emulator, OR check what Azure can do..
             * */

            string content = await _client.GetStringAsync(url);
            var faculty = JsonConvert.DeserializeObject<List<Faculty>>(content);
            

            xList.ItemsSource = new ObservableCollection<Faculty>(faculty);

            base.OnAppearing();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Faculty tapped = e.Item as Faculty;
            (BindingContext as FacultiesListViewModel).FacultySelected(tapped);
            
            await this.Navigation.PushAsync(new Views.NotebooksDB.FacultyCoursesListPage(tapped.Id));

            //Deselect Item
            // I Used OnPropertyChanged within BaseViewModel Class, but something wrong happening
            ((ListView)sender).SelectedItem = null;
        }
    }
}