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
        //public ObservableCollection<Faculty> FacultiesList { get; private set; } = new ObservableCollection<Faculty>
        //{
        //    new Faculty { Id = 1, Name = "Computer Science", Description = "CS Dept"},
        //    new Faculty { Id = 2, Name = "Pharmacy", Description = "Pharmacy Dept"}
        //};

        public async Task<List<Faculty>> GetFacultiesListFromAPI()
        {
            string url = "http://localhost:53626/api/Faculties";
            HttpClient client = new HttpClient();

            var content = await client.GetStringAsync(url);
            var faculties = JsonConvert.DeserializeObject<List<Faculty>>(content);

            return faculties;
        }

        /*
        private readonly IPageService _pageService;
        public FacultiesListViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }
        */

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get { return _selectedFaculty; }
            set { SetValue(ref _selectedFaculty, value); }
        }

        public void FacultySelected(Faculty faculty)
        {
            if (faculty == null)
                return;

            // also return type should be: Task
            //await _pageService.PushAsync(new Views.NotebooksDB.FacultyCoursesListPage());

            //Deselect Item
            SelectedFaculty = null;
        }
        
    }
}
