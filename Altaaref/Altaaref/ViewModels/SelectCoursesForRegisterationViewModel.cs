using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Altaaref.ViewModels
{
    public class SelectCoursesForRegisterationViewModel : BaseViewModel
    {
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

        private List<Faculty> FacultiesSelectedList;

        private Dictionary<int, Courses> CoursesByFaculty = new Dictionary<int, Courses>();

        public SelectCoursesForRegisterationViewModel(IPageService pageService, List<Faculty> FacultiesSelectedList)
        {
            this.FacultiesSelectedList = FacultiesSelectedList;

            foreach(var fc in FacultiesSelectedList)
            {
                var task = GetFacultiesCourses(fc.Id);
            }
        }

        private async Task GetFacultiesCourses(int facultyId)
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/FacultyCourses/GetCoursesByFacultiesId/" + facultyId;

            string content = await _client.GetStringAsync(url);
            var crs = JsonConvert.DeserializeObject<Courses>(content);
            CoursesByFaculty.Add(facultyId, crs);

            Busy = false;
        }
    }
}
