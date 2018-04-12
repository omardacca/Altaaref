using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewInvitation
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }

    public class ViewStudyGroupInvitationsViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;

        private List<ViewInvitation> _viewInvitationList;
        public List<ViewInvitation> ViewInvitationList
        {
            get { return _viewInvitationList; }
            private set
            {
                _viewInvitationList = value;
                OnPropertyChanged(nameof(ViewInvitationList));
            }
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

        

        public ViewStudyGroupInvitationsViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        // NOT READY YET!!
        private async void GetInvitationsListAsync()
        {
            Busy = true;
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations";

            //string content = await _client.GetStringAsync(url);
            //var list = JsonConvert.DeserializeObject<List<Student>>(content);
            //var stdView = new List<ViewInvitation>();
            //foreach (Student std in list)
            //    stdView.Add(new ViewStudent { Student = std });

            //ViewInvitationList = new List<ViewInvitation>(stdView);
            Busy = false;
        }

        private void PostAttendance()
        {

        }

        private void DeleteInvitations()
        {

        }
    }
}
