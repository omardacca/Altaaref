using Altaaref.Models;
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

        public SelectCoursesForRegisterationViewModel(List<Faculty> FacultiesSelectedList)
        {

        }

        private async Task GetFacultiesCourses()
        {
            Busy = true;



            Busy = false;
        }
    }
}
