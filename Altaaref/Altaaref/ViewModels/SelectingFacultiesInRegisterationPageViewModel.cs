using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class SelectingFacultiesInRegisterationPageViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public SelectingFacultiesInRegisterationPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
            FacultiesSelectedList = new List<Faculty>();


            var list = GetFacultiesList();
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

        public ICommand FacultySelectedCommand => new Command(facultySelected);
        public ICommand NextButtonCommand => new Command(async () => await HandleNextButtonTap());

        private List<ViewFaculty> _facultiesList;
        public List<ViewFaculty> FacultiesList
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

        private List<Faculty> FacultiesSelectedList;

        private ViewFaculty _selectedFaculty;
        public ViewFaculty SelectedFaculty
        {
            get { return _selectedFaculty; }
            set { SetValue(ref _selectedFaculty, value); }
        }

        private void facultySelected()
        {
            if (_selectedFaculty != null)
            {
                AddOrRemoveFacultyFromFacultyList(_selectedFaculty.Faculty);
                FacultiesList.Find(vf => vf.Faculty.Id == _selectedFaculty.Faculty.Id)
                    .IsImageVisible = !FacultiesList.Find(vf => vf.Faculty.Id == _selectedFaculty.Faculty.Id).IsImageVisible;
            }
        }

        public void AddOrRemoveFacultyFromFacultyList(Faculty faculty)
        {
            var result = FacultiesSelectedList.Find(s => s.Id == faculty.Id);
            if (result != null)
                FacultiesSelectedList.Remove(result);
            else
                FacultiesSelectedList.Add(faculty);
        }

        private async Task HandleNextButtonTap()
        {
            if(FacultiesSelectedList == null || FacultiesSelectedList.Count == 0)
            {
                await _pageService.DisplayAlert("Error", "Please select at least one faculty!", "Ok", "Cancel");
                return;
            }

           await  _pageService.PushAsync(new Views.SelectCoursesForRegisterationPage(FacultiesSelectedList));
        }

        private async Task GetFacultiesList()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/Faculties";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Faculty>>(content);

            var fcView = new List<ViewFaculty>();
            foreach (Faculty fc in list)
                fcView.Add(new ViewFaculty { Faculty = fc, IsImageVisible = false });

            FacultiesList = new List<ViewFaculty>(fcView);

            Busy = false;
        }
    }
}
