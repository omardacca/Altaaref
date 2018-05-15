using Altaaref.Helpers;
using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels.StudyGroup
{
    public class MainPageViewModel : BaseViewModel
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

        private List<StudyGroupView> _studyGroupsList;
        public List<StudyGroupView> StudyGroupsList
        {
            get { return _studyGroupsList; }
            private set
            {
                _studyGroupsList = value;
                OnPropertyChanged(nameof(StudyGroupsList));
            }
        }

        private List<StudyGroupView> _mysglist;
        public List<StudyGroupView> MySGList
        {
            get
            {
                return _mysglist;
            }
            private set
            {
                _mysglist = value;
                OnPropertyChanged(nameof(MySGList));
            }
        }

        private List<ViewInvitation> _invitationsList;
        public List<ViewInvitation> InvitationsList
        {
            get
            {
                return _invitationsList;
            }
            private set
            {
                _invitationsList = value;
                OnPropertyChanged(nameof(InvitationsList));
            }
        }

        public ICommand AddButtonCommand => new Command(AddAction);
        public ICommand FindButtonCommand => new Command(FindAction);

        private ICommand _viewSGCommand;
        public ICommand ViewSGCommand { get { return _viewSGCommand; } }

        private ICommand _viewInvCommand;
        public ICommand ViewInvitationsCommand { get { return _viewInvCommand; } }

        private ICommand _acceptCommand;
        public ICommand AcceptCommand { get { return _acceptCommand; } }

        public MainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            _viewInvCommand = new Command<ViewInvitation>(ViewInvitationTapped);
            _acceptCommand = new Command<ViewInvitation>(AcceptTapped);
            _viewSGCommand = new Command<StudyGroupView>(ViewSGTapped);

            var task = GetData();
        }

        private async Task GetData()
        {
            await GetInvitationsListAsync();
            await GetStudyGroupsByStudentId();
            await GetMyStudyGroupsList();
        }



        // Ready
        private void PutInvitationVerificationSatus(StudyGroupInvitations UpdatedViewInvitation)
        {
            Busy = true;
            var postUrl = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations/" + Settings.Identity;

            var content = new StringContent(JsonConvert.SerializeObject(UpdatedViewInvitation), Encoding.UTF8, "application/json");
            var response = _client.PutAsync(postUrl, content);

            Busy = false;
        }

        public void AcceptTapped(ViewInvitation vInvitation)
        {
            Busy = true;

            // if clicked to attend - post him
            if (!vInvitation.VerificationStatus)
            {
                PostAttendance(new StudyGroupAttendants { StudentId = Settings.StudentId, StudyGroupId = vInvitation.StudyGroup.StudyGroupId });
                PutInvitationVerificationSatus(new StudyGroupInvitations { StudentId = Settings.StudentId, StudyGroupId = vInvitation.StudyGroup.StudyGroupId, VerificationStatus = true });
            }
            else // not applied it in the VIEW
            {
                PutInvitationVerificationSatus(new StudyGroupInvitations { StudentId = Settings.StudentId, StudyGroupId = vInvitation.StudyGroup.StudyGroupId, VerificationStatus = false });
                DeleteAttendant(vInvitation.StudyGroup.StudyGroupId);
            }
            vInvitation.VerificationStatus = !vInvitation.VerificationStatus;

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

            var url = "https://altaarefapp.azurewebsites.net/api/StudyGroupAttendants/" + StudyGroupId + "/" + Settings.Identity;

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


        public void ViewInvitationTapped(ViewInvitation invitation)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(invitation.StudyGroup));
        }

        public void ViewSGTapped(StudyGroupView studyGroup)
        {
            _pageService.PushAsync(new Views.StudyGroups.ViewStudyGroupDetails(studyGroup));
        }



        private void AddAction()
        {
            _pageService.PushAsync(new Views.StudyGroups.NewGroupPage());
        }

        private void FindAction()
        {
            _pageService.PushAsync(new Views.StudyGroups.FindStudyGroup());
        }

        private async Task GetInvitationsListAsync()
        {
            Busy = true;
            var url = "https://altaarefapp.azurewebsites.net/api/StudyGroupInvitations/" + Settings.Identity;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ViewInvitation>>(content);
            InvitationsList = new List<ViewInvitation>(list);

            //if (ViewInvitationList == null || ViewInvitationList.Count == 0)
            //    IsListEmpty = true;

            Busy = false;
        }

        private async Task GetStudyGroupsByStudentId()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + Settings.Identity;

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroupView>>(content);
            StudyGroupsList = list;

            Busy = false;
        }

        private async Task GetMyStudyGroupsList()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById/" + Settings.Identity;

            string results = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<StudyGroupView>>(results);
            MySGList = new List<StudyGroupView>(list);

//            if (MySGList == null || MySGList.Count == 0)
//                IsListEmpty = true;

            Busy = false;
        }


    }
}
