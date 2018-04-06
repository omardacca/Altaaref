using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Altaaref.ViewModels
{
    public class ViewStudyGroupDetailsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private StudyGroup _studyGroup;
        public StudyGroup StudyGroup
        {
            get { return _studyGroup; }
            private set { SetValue(ref _studyGroup, value); }
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

        private Courses _course;
        public Courses Course
        {
            get { return _course; }
            private set { SetValue(ref _course, value); }
        }

        private Student _student;
        public Student Student
        {
            get { return _student; }
            private set { SetValue(ref _student, value); }
        }

        public ViewStudyGroupDetailsViewModel(StudyGroup studyGroup, IPageService pageService)
        {
            _pageService = pageService;
            StudyGroup = studyGroup;

            GetCourseByIdAsync(studyGroup.CourseId);
            GetStudentByIdAsync(studyGroup.StudentId);
        }

        private async void GetCourseByIdAsync(int courseId)
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/Courses/" + courseId;

            string content = await _client.GetStringAsync(url);
            var course = JsonConvert.DeserializeObject<Courses>(content);
            Course = course;
        }

        private async void GetStudentByIdAsync(int studentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/Students/" + studentId;

            string content = await _client.GetStringAsync(url);
            var student = JsonConvert.DeserializeObject<Student>(content);
            Student = student;

            Busy = false;
        }
    }
}
