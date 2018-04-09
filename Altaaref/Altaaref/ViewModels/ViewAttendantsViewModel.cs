using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewAttendantsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public ViewAttendantsViewModel(IPageService pageService, int StudyGroupId)
        {
            _pageService = pageService;
        }


    }
}
