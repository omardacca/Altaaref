using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Altaaref.ViewModels
{
    public class ViewInvitation : BaseViewModel
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public Models.StudyGroup StudyGroup { get; set; }

        private bool _VerificationStatus;
        public bool VerificationStatus
        {
            get { return _VerificationStatus; }
            set { SetValue(ref _VerificationStatus, value); }
        }
    }

    public class ViewStudyGroupInvitationsViewModel : BaseViewModel
    {
        int StudentId = 204228043;

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
        
        private ViewInvitation _selectedViewStudyGroupt;
        public ViewInvitation SelectedViewStudyGroup
        {
            get { return _selectedViewStudyGroupt; }
            set { SetValue(ref _selectedViewStudyGroupt, value); }
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
            GetInvitationsListAsync();
        }

        private async void GetInvitationsListAsync()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewInvitation>>(content);
            ViewInvitationList = new List<ViewInvitation>(list);
            Busy = false;
        }

        private async void PostAttendance(StudyGroupAttendants attendant)
        {
            Busy = true;
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants";

            var content = new StringContent(JsonConvert.SerializeObject(attendant), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(postUrl, content);

            if (response.Result.IsSuccessStatusCode)
            {
                //await _pageService.DisplayAlert("Students Invited", "Students Invited Successfully", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with Posting Attendants", "OK", "Cancel");
            }

            Busy = false;
        }

        private async void DeleteAttendant(int StudyGroupId)
        {
            Busy = true;

            var url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/" + StudyGroupId + "/" + StudentId;

            var response = _client.DeleteAsync(url);

            if (response.Result.IsSuccessStatusCode)
            {
                //await _pageService.DisplayAlert("Students Invited", "Students Invited Successfully", "OK", "Cancel");
            }
            else
            {
                await _pageService.DisplayAlert("Error", "Something went wrong with Delete Attendants", "OK", "Cancel");
            }

            Busy = false;
        }

        private void PutInvitationVerificationSatus(StudyGroupInvitations UpdatedViewInvitation)
        {
            Busy = true;
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations/" + UpdatedViewInvitation.StudentId;

            var content = new StringContent(JsonConvert.SerializeObject(UpdatedViewInvitation), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(postUrl, content);

            Busy = false;
        }

        public void ViewInvitationSelected(ViewInvitation vInvitation)
        {
            ////Deselect Item
            SelectedViewStudyGroup = null;

            // if clicked to attend - post him
            if(!vInvitation.VerificationStatus)
            {
                PostAttendance(new StudyGroupAttendants { StudentId = StudentId, StudyGroupId = vInvitation.StudyGroup.Id });
                PutInvitationVerificationSatus(new StudyGroupInvitations { StudentId = StudentId, StudyGroupId = vInvitation.StudyGroup.Id, VerificationStatus = true });
            }
            else
            {
                PutInvitationVerificationSatus(new StudyGroupInvitations { StudentId = StudentId, StudyGroupId = vInvitation.StudyGroup.Id, VerificationStatus = false });
                DeleteAttendant(vInvitation.StudyGroup.Id);
            }
            vInvitation.VerificationStatus = !vInvitation.VerificationStatus;

        }
    }
}
