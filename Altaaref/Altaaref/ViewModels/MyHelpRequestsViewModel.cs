using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class MyHelpRequestsViewModel : BaseViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                SetValue(ref _busy, value);
            }
        }

        public MyHelpRequestsViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        private async void InitStudentListByCourseIdAsync(int courseId)
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudentCourses/" + courseId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Student>>(content);
            var stdView = new List<Student>();

            //StudentsList = new List<Student>(stdView);
            Busy = false;
        }

    }
}
