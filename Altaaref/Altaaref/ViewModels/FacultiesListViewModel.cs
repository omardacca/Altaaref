using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                
        private readonly IPageService _pageService;
        public FacultiesListViewModel(IPageService pageService)
        {
            _pageService = pageService;
            GetFacultiesListFromAPI();
        }

        private async void GetFacultiesListFromAPI()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Faculties";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Faculty>>(content);
            FacultiesList = new ObservableCollection<Faculty>(list);
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
