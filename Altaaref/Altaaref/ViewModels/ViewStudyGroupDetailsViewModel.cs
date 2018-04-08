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

        private bool _isAttendant;
        public bool IsAttendant
        {
            get { return _isAttendant; }
            set
            {
                SetValue(ref _isAttendant, value);
            }
        }


        public ICommand HandlePostAttendatCommand { get; private set; }
        public ICommand HandleRemoveAttendantCommand { get; private set; }

        public ViewStudyGroupDetailsViewModel(StudyGroup studyGroup, IPageService pageService)
        {
            _pageService = pageService;
            StudyGroup = studyGroup;
            HandlePostAttendatCommand = new Command(HandlePostAttendant);
            HandleRemoveAttendantCommand = new Command(HandleRemoveAttendant);

            GetCourseByIdAsync(studyGroup.CourseId);
            GetStudentByIdAsync(studyGroup.StudentId);
            // IsAttendant = ; check if current user is attendant 
            // (check if StudentId and StudyGroupId exist in StudyGroupAttendant table)
            
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

        private void HandlePostAttendant()
        {
            PostAttendanceAsync();
            IsAttendant = true;
        }

        private void HandleRemoveAttendant()
        {
            RemoveAttendanceAsync();
            IsAttendant = false;
        }

        private async void PostAttendanceAsync()
        {
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants";

            var sga = new StudyGroupAttendants { StudentId = StudyGroup.StudentId, StudyGroupId = StudyGroup.Id };

            var content = new StringContent(JsonConvert.SerializeObject(sga), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            var StudyGroupInserted = JsonConvert.DeserializeObject<StudyGroupAttendants>(await response.Result.Content.ReadAsStringAsync());


            if (response.Result.IsSuccessStatusCode)
            {
                await _pageService.DisplayAlert("Add Attendant", "You have been added to the Group. ", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with Add Study Group attendant", "OK", "Cancel");
            }
        }

        private void RemoveAttendanceAsync()
        {
            // Not Implemented yet
        }
    }
}
