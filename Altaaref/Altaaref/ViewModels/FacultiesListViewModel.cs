using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class FacultiesListViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private ObservableCollection<Faculty> _facultiesList;
        public ObservableCollection<Faculty> FacultiesList
        {
            get
            {
                return _facultiesList;
            }
            private set
            {
                _facultiesList = value;
                OnPropertyChanged(nameof(FacultiesList));
            }
        }

        //private string _searchIcon;
        //public string SearchIcon
        //{
        //    get { return _searchIcon; }
        //    set { SetValue(ref _searchIcon, value); }
        //}

        public ICommand SearchImageButtonCommand { get; private set; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetValue(ref _searchText, value); }
        }



        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        private readonly IPageService _pageService;
        public FacultiesListViewModel(IPageService pageService)
        {
            // Enable activity Indicator - disable is right after assigning listView item source
            Busy = true;

            SearchImageButtonCommand = new Command(SearchImageButtonTapped);

            _pageService = pageService;

            GetFacultiesListFromAPI();
        }

        private void SearchImageButtonTapped()
        {

        }

        private async void GetFacultiesListFromAPI()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Faculties";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Faculty>>(content);
            FacultiesList = new ObservableCollection<Faculty>(list);

            // Disable Activity Idicator
            Busy = false;
        }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get { return _selectedFaculty; }
            set { SetValue(ref _selectedFaculty, value); }
        }

        public async Task FacultySelectedAsync(Faculty faculty)
        {
            //Deselect Item
            SelectedFaculty = null;
            
            await _pageService.PushAsync(new Views.NotebooksDB.FacultyCoursesListPage(faculty.Id));
        }
        
    }
}
