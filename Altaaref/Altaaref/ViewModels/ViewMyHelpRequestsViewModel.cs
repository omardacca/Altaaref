using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewMyHelpRequestsViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        public ViewMyHelpRequestsViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }
    }
}
