﻿using Altaaref.Models;
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

    public class ViewFaculty : BaseViewModel
    {
        private Faculty _faculty;
        public Faculty Faculty
        {
            get { return _faculty; }
            set
            {
                _faculty = value;
                OnPropertyChanged(nameof(Faculty));
            }
        }

        private bool _IsImageVisible;
        public bool IsImageVisible
        {
            get { return _IsImageVisible; }
            set { SetValue(ref _IsImageVisible, value); }
        }
    }

    public class SelectHelpRequestFacultiesViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

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

        private HelpRequest newHelpRequest;

        private List<Faculty> FacultiesSelectedList;

        private ViewFaculty _selectedFaculty;
        public ViewFaculty SelectedFaculty
        {
            get { return _selectedFaculty; }
            set { SetValue(ref _selectedFaculty, value); }
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

        public ICommand SubmitButtonCommand { get; set; }

        public SelectHelpRequestFacultiesViewModel(IPageService pageService, HelpRequest newHelpRequest)
        {
            _pageService = pageService;
            this.newHelpRequest = newHelpRequest;

            SubmitButtonCommand = new Command(OnSubmitButtonTapped);

            FacultiesSelectedList = new List<Faculty>();

            GetCoursesAsync();
        }

        private List<HelpFaculty> InitHelpFacultiesListBasedOnSelected(int HelpRequestId)
        {
            List<HelpFaculty> HFList = new List<HelpFaculty>();
            foreach (var faculty in FacultiesSelectedList)
                HFList.Add(new HelpFaculty { HelpRequestId = HelpRequestId, FacultyId = faculty.Id });

            return HFList;
        }

        private async void OnSubmitButtonTapped(object obj)
        {
            var HelpRequestId = await PostHelpRequest();
            List<HelpFaculty> helpFacultiesObjects = InitHelpFacultiesListBasedOnSelected(HelpRequestId);
            await PostHelpRequestFaculty(helpFacultiesObjects);
        }

        public void FacultySelected(ViewFaculty faculty)
        {
            //Deselect Item
            SelectedFaculty = null;

            AddOrRemoveFacultyFromFacultyList(faculty.Faculty);
            FacultiesList.Find(vf => vf.Faculty.Id == faculty.Faculty.Id)
                .IsImageVisible = !FacultiesList.Find(vf => vf.Faculty.Id == faculty.Faculty.Id).IsImageVisible;
        }

        public void AddOrRemoveFacultyFromFacultyList(Faculty faculty)
        {
            var result = FacultiesSelectedList.Find(s => s.Id == faculty.Id);
            if (result != null)
                FacultiesSelectedList.Remove(result);
            else
                FacultiesSelectedList.Add(faculty);
        }

        private async Task PostHelpRequestFaculty(List<HelpFaculty> helpFacultiesObjects)
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/HelpFaculties";

            var content = new StringContent(JsonConvert.SerializeObject(helpFacultiesObjects), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var InsertedHelpRequestId = JsonConvert.DeserializeObject<HelpFaculty>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Created Successfully", "Help Request Created Successfully", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
            }
        }

        private async Task<int> PostHelpRequest()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/HelpRequests";

            var content = new StringContent(JsonConvert.SerializeObject(newHelpRequest), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var InsertedHelpRequestId = JsonConvert.DeserializeObject<HelpRequest>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Created Successfully", "Help Request Created Successfully", "OK", "Cancel");
                return InsertedHelpRequestId.Id;
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
                return -1;
            }
        }

        private async void GetCoursesAsync()
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
