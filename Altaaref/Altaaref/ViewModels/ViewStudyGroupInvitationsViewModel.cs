using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class ViewInvitation : BaseViewModel
    {
        public StudyGroupView StudyGroup { get; set; }

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

        private ICommand _viewInvitationscommand;
        public ICommand ViewInvitationsCommand { get { return _viewInvitationscommand; } }

        private ICommand _acceptCommand;
        public ICommand AcceptCommand { get { return _acceptCommand; } }

        private bool _isListEmpty;
        public bool IsListEmpty
        {
            get { return _isListEmpty; }
            set
            {
                SetValue(ref _isListEmpty, value);
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
            _viewInvitationscommand = new Command<ViewInvitation>(ViewTapped);
            _acceptCommand = new Command<ViewInvitation>(AcceptTapped);

            GetInvitationsListAsync();
        }

        private async void GetInvitationsListAsync()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations/" + StudentId;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewInvitation>>(content);
            ViewInvitationList = new List<ViewInvitation>(list);

            if (ViewInvitationList == null || ViewInvitationList.Count == 0)
                IsListEmpty = true;

            Busy = false;
        }


        //  Ready
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


        // Ready
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


        // Ready
        private void PutInvitationVerificationSatus(StudyGroupInvitations UpdatedViewInvitation)
        {
            Busy = true;
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations/" + UpdatedViewInvitation.StudentId;

            var content = new StringContent(JsonConvert.SerializeObject(UpdatedViewInvitation), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(postUrl, content);

            Busy = false;
        }


        // Ready
        public void AcceptTapped(ViewInvitation vInvitation)
        {
            // if clicked to attend - post him
            if(!vInvitation.VerificationStatus)
            {
                PostAttendance(new StudyGroupAttendants { StudentId = StudentId, StudyGroupId = vInvitation.StudyGroup.StudyGroupId});
                PutInvitationVerificationSatus(new StudyGroupInvitations { StudentId = StudentId, StudyGroupId = vInvitation.StudyGroup.StudyGroupId, VerificationStatus = true });
            }
            else
            {
                PutInvitationVerificationSatus(new StudyGroupInvitations { StudentId = StudentId, StudyGroupId = vInvitation.StudyGroup.StudyGroupId, VerificationStatus = false });
                DeleteAttendant(vInvitation.StudyGroup.StudyGroupId);
            }
            vInvitation.VerificationStatus = !vInvitation.VerificationStatus;
            
        }

        public void ViewTapped(ViewInvitation Inv)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(Inv.StudyGroup));
        }
    }
}
