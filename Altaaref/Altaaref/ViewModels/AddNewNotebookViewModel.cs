using Altaaref.Helpers;
using Altaaref.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class AddNewNotebookViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private List<Courses> _coursesList;
        public List<Courses> CoursesList
        {
            get { return _coursesList; }
            private set
            {
                _coursesList = value;
                OnPropertyChanged(nameof(CoursesList));
            }
        }

        private List<string> _coursesNamesList;
        public List<string> CoursesNameList
        {
            get { return _coursesNamesList; }
            set
            {
                _coursesNamesList = value;
                OnPropertyChanged(nameof(CoursesNameList));
            }
        }

        private string _titleEntry;
        public string TitleEntry
        {
            get { return _titleEntry; }
            set { SetValue(ref _titleEntry, value); }
        }

        //public ICommand HandleSubmition { get; private set; }
        public ICommand UploadCommand => new Command(async () => await UploadToBlob());
        public ICommand ToggleCommand => new Command(async () => await HandlePublicCommand());

        private int _selectedCourseIndex;
        public int SelectedCourseIndex
        {
            get { return _selectedCourseIndex; }
            set { SetValue(ref _selectedCourseIndex, value); }
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

        private bool _isGeneralToggled;
        public bool IsGeneralToggled
        {
            get { return _isGeneralToggled; }
            set
            {
                SetValue(ref _isGeneralToggled, value);
            }
        }

        private readonly IPageService _pageService;

        public AddNewNotebookViewModel(IPageService pageService)
        {
            _pageService = pageService;
            CoursesNameList = new List<string>();

            Init();
        }

        async void Init()
        {
            Busy = true;
            await GetCoursesAsync();
            InitCoursesList();
            Busy = false;
        }

        private void InitCoursesList()
        {
            foreach (var c in CoursesList)
                CoursesNameList.Add(c.Name);
        }

        private async Task GetCoursesAsync()
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Courses";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            CoursesList = new List<Courses>(list);
        }

        private async Task UploadToBlob()
        {
            var courseid = _coursesList[_selectedCourseIndex].Id;
            var titleEntry = TitleEntry;

            await DependencyService.Get<IUploader>().UploadToBlob(courseid, titleEntry, Settings.StudentId);

            if (!IsGeneralToggled)
                await PutToggledFalse();

            await _pageService.DisplayAlert("Upload Success", "Notebook Added Successfully.", "Ok", "Cancel");

            await FCMPushNotificationSender.Send(
                "NS" + courseid, "New Notebook", "New notebook that you may interest in has been added");

            await _pageService.PopAsync();
        }

        public async Task HandlePublicCommand()
        {

        }

        private async Task PutToggledFalse()
        {
            //var puttUrl = "https://altaarefapp.azurewebsites.net/api/Notebooks/"

            // notebook init and toggle false

            //var content = new StringContent(JsonConvert.SerializeObject(UpdatedRideInvitaion), Encoding.UTF8, "application/json");
            //var response = await _client.PutAsync(puttUrl, content);

            //return response.IsSuccessStatusCode;
        }



    }
}
