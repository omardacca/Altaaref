using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class MyStudyGroupsViewModel
    {
        int StudentId = 204228043;

        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public MyStudyGroupsViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }


    }
}
