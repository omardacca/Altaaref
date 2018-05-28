using Altaaref.Helpers;
using Altaaref.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.Models
{

    public class NotificationFacultyBasedViewModel : BaseViewModel
    {
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;
        private readonly NotificationSettingsViewModelType _modelType;

        private List<UserNotification> _notificationList;
        private List<UserNotification> NotificationList
        {
            get
            {
                return _notificationList;
            }
            set
            {
                _notificationList = value;
                OnPropertyChanged(nameof(NotificationList));
            }
        }

        private List<Faculty> _facultyList;
        public List<Faculty> FacultyList
        {
            get
            {
                return _facultyList;
            }
            set
            {
                _facultyList = value;
                OnPropertyChanged(nameof(FacultyList));
            }
        }

        private List<Faculty> _notificationFaculty;
        public List<Faculty> NotificationFaculty
        {
            get
            {
                return _notificationFaculty;
            }
            set
            {
                _notificationFaculty = value;
                OnPropertyChanged(nameof(NotificationFaculty));
            }
        }

        private string _emptyListLabel;
        public string EmptyListLabel
        {
            get { return _emptyListLabel; }
            set { SetValue(ref _emptyListLabel, value); }
        }

        private string _titleLabel;
        public string TitleLabel
        {
            get { return _titleLabel; }
            set { SetValue(ref _titleLabel, value); }
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

        private bool _isNotificationListEmpty;
        public bool IsNotificationListEmpty
        {
            get { return _isNotificationListEmpty; }
            set
            {
                SetValue(ref _isNotificationListEmpty, value);
            }
        }

        private bool _isPickerListEmpty;
        public bool IsPickerListEmpty
        {
            get { return _isPickerListEmpty; }
            set
            {
                SetValue(ref _isPickerListEmpty, value);
            }
        }

        private int _facultyId;

        private int _selectedFacultyIndex;
        public int SelectedFacultyIndex
        {
            get { return _selectedFacultyIndex; }
            set { SetValue(ref _selectedFacultyIndex, value); }
        }

        public ICommand AddCommand => new Command(async () => { await HandleAddTap(); });
        public ICommand RemoveCommand => new Command<Faculty>(async (Faculty) => { await HandleRemoveTap(Faculty); });

        public NotificationFacultyBasedViewModel(IPageService pageService, NotificationSettingsViewModelType modelType)
        {
            _pageService = pageService;
            _modelType = modelType;

            InitNotificationList();

            var init = InitLists();
        }

        private async Task InitLists()
        {
            NotificationFaculty = new List<Faculty>();

            await GetStudentFaculties();

            FilterFacultiesList();
        }

        private void InitNotificationList()
        {
            string serializedList = Application.Current.Properties["SerializedUserNotif"] as string;
            var list = JsonConvert.DeserializeObject<List<UserNotification>>(serializedList);

            NotificationList = list;

            switch (_modelType)
            {
                case NotificationSettingsViewModelType.MutualHelpFaculty:
                    TitleLabel = "Add faculty you would like to get notify when anyone add Help Request related to it";
                    EmptyListLabel = "You still don't have any Mutual Help related notification to any of your faculties.";
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("HRF"));
                    break;
            }

            if (NotificationList.Count == 0) IsNotificationListEmpty = true;

        }

        private async Task GetStudentFaculties()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFaculties/GetFaculties/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            FacultyList = JsonConvert.DeserializeObject<List<Faculty>>(content);

            Busy = false;
        }

        private void FilterFacultiesList()
        {
            // Note: note that if notification list include General or Custom Notification
            // (not one of XX prefix) then it will not be included here, because its already
            // filtered in the switch cases in the constructor based on the modelType

            if (FacultyList.Count == 0)
            {
                IsPickerListEmpty = false;
                return;
            }
            else IsPickerListEmpty = true;

            if (NotificationList.Count == 0)
                return;

            IsNotificationListEmpty = false;

            List<int> facultiesIdFromSettings = new List<int>();
            foreach (var crs in NotificationList)
                facultiesIdFromSettings.Add(int.Parse(crs.Topic.Substring(2)));

            List<Faculty> templist = new List<Faculty>();
            Faculty tempFaculty;
            foreach (var fclt in facultiesIdFromSettings)
            {
                tempFaculty = FacultyList.Find(c => c.Id == fclt);
                templist.Add(tempFaculty);
                FacultyList.Remove(tempFaculty);
            }

            NotificationFaculty = templist;

            if (FacultyList.Count == 0)
            {
                IsPickerListEmpty = false;
                return;
            }
            else IsPickerListEmpty = true;
        }

        private async Task HandleAddTap()
        {
            _facultyId = _facultyList[_selectedFacultyIndex].Id;

            switch (_modelType)
            {
                case NotificationSettingsViewModelType.MutualHelpFaculty:
                    await HandleHRFacultyAddButton();
                    break;
            }

            await GetNotifications();

            await InitLists();
        }

        private async Task HandleHRFacultyAddButton()
        {
            // Subscribe to topic
            DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("HR" + _facultyId);

            // Register StudentId with the notification in the db, and update the list in App Properties
            var insertedUserNotification = await FCMPushNotificationSender.AddNotification(
                new UserNotification
                {
                    StudentId = Settings.StudentId,
                    Topic = "HRF" + _facultyId,
                    Title = "Help",
                    Body = "Someone asked for help in your faculty"
                });

            // update the SG list in this page
            if (insertedUserNotification != null)
                NotificationList.Add(insertedUserNotification);

        }

        private async Task HandleRemoveTap(Faculty faculty)
        {
            await DeleteNotification(faculty);

            var temp = NotificationFaculty;
            temp.Remove(faculty);
            NotificationFaculty = temp;

            var topic = NotificationList.Find(un => un.Topic.Contains(faculty.Id.ToString())).Topic;

            DependencyService.Get<IFCMNotificationSubscriber>()
                .UnSubscribe(topic);

            await GetNotifications();

            InitNotificationList();

            await InitLists();
        }

        private async Task DeleteNotification(Faculty faculty)
        {
            Busy = true;

            var id = _notificationList.Find(n => n.Topic.Contains(faculty.Id.ToString())).Id;

            var url = "https://altaarefapp.azurewebsites.net/api/UserNotifications/" + id;

            try
            {
                var content = await _client.DeleteAsync(url);

                Busy = false;
            }
            catch (HttpRequestException e)
            {

                Busy = false;
            }

            Busy = false;
        }

        private async Task GetNotifications()
        {
            Busy = true;

            string url = "https://altaarefapp.azurewebsites.net/api/UserNotifications/GetByStudentId/" + Settings.StudentId;

            try
            {
                string results = await _client.GetStringAsync(url);
                var list = JsonConvert.DeserializeObject<List<UserNotification>>(results);

                if (list != null && list.Count != 0)
                    Application.Current.Properties["SerializedUserNotif"] = results;
                else
                    Application.Current.Properties["SerializedUserNotif"] = null;

                await Application.Current.SavePropertiesAsync();

                // Subscribe to topics
                foreach (var un in list)
                    DependencyService.Get<IFCMNotificationSubscriber>().Subscribe(un.Topic);

                Busy = false;

            }
            catch (Exception e)
            {
                Application.Current.Properties["SerializedUserNotif"] = null;
                await Application.Current.SavePropertiesAsync();

                Busy = false;
            }

            Busy = false;
        }

    }
}
