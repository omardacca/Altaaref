using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class ViewStudent : BaseViewModel
    {
        private Student _student;
        public Student Student
        {
            get { return _student; }
            set
            {
                _student = value;
                OnPropertyChanged(nameof(Student));
            }
        }

        private bool _IsImageVisible;
        public bool IsImageVisible
        {
            get { return _IsImageVisible; }
            set { SetValue(ref _IsImageVisible, value); }
        }

    }

    public class SelectStudentsToInviteViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();

        private readonly IPageService _pageService;

        private readonly int studyGroupId;

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        private List<StudyGroupInvitations> StudentsToInviteList;

        private List<ViewStudent> _studentList;
        public List<ViewStudent> StudentsList
        {
            get { return _studentList; }
            private set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentsList));
            }
        }

        private ViewStudent _selectedStudent;
        public ViewStudent SelectedStudent
        {
            get { return _selectedStudent; }
            set { SetValue(ref _selectedStudent, value); }
        }

        public ICommand AddButtonCommand { get; private set; }
        public ICommand SubmitButtonCommand { get; set; }

        public SelectStudentsToInviteViewModel(IPageService pageService, int courseId, int studyGroupId)
        {
            this.studyGroupId = studyGroupId;
            _pageService = pageService;
            StudentsToInviteList = new List<StudyGroupInvitations>();
            SubmitButtonCommand = new Command(OnSubmitButtonTapped);
            InitStudentListByCourseIdAsync(courseId);
        }
        
        private void OnSubmitButtonTapped(object obj)
        {
            PostSelectedStudent();
        }

        private async void PostSelectedStudent()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations";

            var content = new StringContent(JsonConvert.SerializeObject(StudentsToInviteList), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Students Invited", "Students Invited Successfully", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong", "OK", "Cancel");
            }
        }

        private async void InitStudentListByCourseIdAsync(int courseId)
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudentCourses/" + courseId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Student>>(content);
            var stdView = new List<ViewStudent>();
            foreach(Student std in list)
                stdView.Add(new ViewStudent { Student = std, IsImageVisible = false });

            StudentsList = new List<ViewStudent>(stdView);
            Busy = false;
        }

        public void StudentSelectedAsync(ViewStudent vstudent)
        {
            //Deselect Item
            SelectedStudent = null;
            
            AddOrRemoveStudentFromInviteList(vstudent.Student.Id);
            StudentsList.Find(vs => vs.Student.Id == vstudent.Student.Id)
                .IsImageVisible = !StudentsList.Find(vs => vs.Student.Id == vstudent.Student.Id).IsImageVisible;
        }
        
        public void AddOrRemoveStudentFromInviteList(int Id)
        {
            var result = StudentsToInviteList.Find(s => s.StudentId == Id);
            if (result != null)
                StudentsToInviteList.Remove(result);
            else
                StudentsToInviteList.Add(
                    new StudyGroupInvitations
                    {
                        StudentId = Id,
                        StudyGroupId = this.studyGroupId,
                        VerificationStatus = false
                    });
        }
    }
}
