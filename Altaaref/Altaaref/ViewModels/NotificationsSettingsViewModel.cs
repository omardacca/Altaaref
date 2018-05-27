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

namespace Altaaref.ViewModels
{
    public enum NotificationSettingsViewModelType { StudyGroup, NotebooksStorage, MutualHelpCourse, MutualHelpFaculty }

    public class NotificationsSettingsViewModel : BaseViewModel
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

        private List<StudentStudiesBase> _pickerList;
        public List<StudentStudiesBase> PickerList
        {
            get
            {
                return _pickerList;
            }
            set
            {
                _pickerList = value;
                OnPropertyChanged(nameof(PickerList));
            }
        }

        private List<Courses> _notificationCourses;
        public List<Courses> NotificationCourses
        {
            get
            {
                return _notificationCourses;
            }
            set
            {
                _notificationCourses = value;
                OnPropertyChanged(nameof(NotificationCourses));
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

        private int _courseId;

        private int _selectedCourseIndex;
        public int SelectedCourseIndex
        {
            get { return _selectedCourseIndex; }
            set { SetValue(ref _selectedCourseIndex, value); }
        }

        public ICommand AddCommand => new Command(async () => { await HandleAddTap(); });
        public ICommand RemoveCommand => new Command<Courses>(async (Courses) => { await HandleRemoveTap(Courses); });

        public NotificationsSettingsViewModel(IPageService pageService, NotificationSettingsViewModelType modelType)
        {
            _pageService = pageService;
            _modelType = modelType;

            InitNotificationList();

            var init = InitLists();
        }

        private async Task InitLists()
        {
            NotificationCourses = new List<Courses>();

            if (_modelType == NotificationSettingsViewModelType.MutualHelpFaculty)
                await GetStudentFaculties();
            else
                await GetStudentCourses();

            FilterPickerList();
        }

        private void InitNotificationList()
        {
            string serializedList = Application.Current.Properties["SerializedUserNotif"] as string;
            var list = JsonConvert.DeserializeObject<List<UserNotification>>(serializedList);

            NotificationList = list;

            switch (_modelType)
            {
                case NotificationSettingsViewModelType.MutualHelpCourse:
                    TitleLabel = "Add course you would like to get notify when anyone add Help Request related to it";
                    EmptyListLabel = "You still don't have any Mutual Help related notifications.";
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("HRC"));
                    break;
                case NotificationSettingsViewModelType.MutualHelpFaculty:
                    TitleLabel = "Add course you would like to get notify when anyone add Help Request related to it";
                    EmptyListLabel = "You still don't have any Mutual Help related notifications.";
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("HRF"));
                    break;
                case NotificationSettingsViewModelType.NotebooksStorage:
                    TitleLabel = "Add course you would like to get notify when anyone add Notebook related to it";
                    EmptyListLabel = "You still don't have any Notebooks related notifications.";
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("NS"));
                    break;
                case NotificationSettingsViewModelType.StudyGroup:
                    TitleLabel = "Add course you would like to get notify when anyone add Study Group related to it";
                    EmptyListLabel = "You still don't have any Mutual Help related notifications.";
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("SG"));
                    break;
            }

            if (NotificationList.Count == 0) IsNotificationListEmpty = true;

        }

        private async Task GetStudentCourses()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudentCourses/GetCourses/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            PickerList = JsonConvert.DeserializeObject<List<StudentStudiesBase>>(content);

            Busy = false;
        }

        private async Task GetStudentFaculties()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/StudentFaculties/GetFaculties/" + Settings.StudentId;

            string content = await _client.GetStringAsync(url);
            PickerList = JsonConvert.DeserializeObject<List<StudentStudiesBase>>(content);

            foreach (var s in PickerList) await _pageService.DisplayAlert("we", s.Name, "ok", "cancel");

            Busy = false;
        }

        private void FilterPickerList()
        {
            // Note: note that if notification list include General or Custom Notification
            // (not one of XX prefix) then it will not be included here, because its already
            // filtered in the switch cases in the constructor based on the modelType

            if (PickerList.Count == 0)
            {
                IsPickerListEmpty = false;
                return;
            }
            else IsPickerListEmpty = true;

            if (NotificationList.Count == 0)
                return;
            
            IsNotificationListEmpty = false;

            List<int> coursesIdFromSettings = new List<int>();
            foreach(var crs in NotificationList)
                coursesIdFromSettings.Add(int.Parse(crs.Topic.Substring(2)));

            List<Courses> templist = new List<Courses>();
            Courses tempCourse;
            foreach(var crs in coursesIdFromSettings)
            {
                tempCourse = PickerList.Find(c => c.Id == crs) as Courses;
                templist.Add(tempCourse);
                PickerList.Remove(tempCourse);
            }

            NotificationCourses = templist;

            if (PickerList.Count == 0)
            {
                IsPickerListEmpty = false;
                return;
            }
            else IsPickerListEmpty = true;
        }

        private async Task HandleAddTap()
        {
            _courseId = _pickerList[_selectedCourseIndex].Id;

            switch (_modelType)
            {
                case NotificationSettingsViewModelType.MutualHelpCourse:
                    await HandleHRCourseAddButton();
                    break;
                case NotificationSettingsViewModelType.MutualHelpFaculty:
                    await HandleHRCourseAddButton();
                    break;
                case NotificationSettingsViewModelType.NotebooksStorage:
                    await HandleNRAddButton();
                    break;
                case NotificationSettingsViewModelType.StudyGroup:
                    await HandleSGAddButton();
                    break;
            }

            await GetNotifications();

            await InitLists();
        }

        private async Task HandleSGAddButton()
        {
            // Subscribe to topic
            DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("SG" + _courseId);

            // Register StudentId with the notification in the db, and update the list in App Properties
            var insertedUserNotification = await FCMPushNotificationSender.AddNotification(
                new UserNotification
                {
                    StudentId = Settings.StudentId,
                    Topic = "SG" + _courseId,
                    Title = "New Study Group",
                    Body = "New study group added in a course you interested in, join now!"
                });

            // update the SG list in this page
            if (insertedUserNotification != null)
                NotificationList.Add(insertedUserNotification);
        }

        private async Task HandleHRCourseAddButton()
        {
            // Subscribe to topic
            DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("HR" + _courseId);

            // Register StudentId with the notification in the db, and update the list in App Properties
            var insertedUserNotification = await FCMPushNotificationSender.AddNotification(
                new UserNotification
                {
                    StudentId = Settings.StudentId,
                    Topic = "HRC" + _courseId,
                    Title = "Help!",
                    Body = "Some need help in a course you attend in, you may give him a help hand"
                });

            // update the SG list in this page
            if (insertedUserNotification != null)
                NotificationList.Add(insertedUserNotification);

        }

        private async Task HandleHRFacultyAddButton()
        {
            // Subscribe to topic
            DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("HR" + _courseId);

            // Register StudentId with the notification in the db, and update the list in App Properties
            var insertedUserNotification = await FCMPushNotificationSender.AddNotification(
                new UserNotification
                {
                    StudentId = Settings.StudentId,
                    Topic = "HRF" + _courseId,
                    Title = "Help",
                    Body = "Someone asked for help in your faculty"
                });

            // update the SG list in this page
            if (insertedUserNotification != null)
                NotificationList.Add(insertedUserNotification);

        }

        private async Task HandleNRAddButton()
        {
            // Subscribe to topic
            DependencyService.Get<IFCMNotificationSubscriber>().Subscribe("NS" + _courseId);

            // Register StudentId with the notification in the db, and update the list in App Properties
            var insertedUserNotification = await FCMPushNotificationSender.AddNotification(
                new UserNotification
                {
                    StudentId = Settings.StudentId,
                    Topic = "NS" + _courseId,
                    Title = "New Notebook",
                    Body = "New notebook that you may interest in has been added"
                });

            // update the SG list in this page
            if (insertedUserNotification != null)
                NotificationList.Add(insertedUserNotification);

        }

        private async Task HandleRemoveTap(Courses course)
        {
            await DeleteNotification(course);

            var temp = NotificationCourses;
            temp.Remove(course);
            NotificationCourses = temp;

            var topic = NotificationList.Find(un => un.Topic.Contains(course.Id.ToString())).Topic;

            DependencyService.Get<IFCMNotificationSubscriber>()
                .UnSubscribe(topic);

            await GetNotifications();

            InitNotificationList();

            await InitLists();
        }

        private async Task DeleteNotification(Courses course)
        {
            Busy = true;

            var id = _notificationList.Find(n => n.Topic.Contains(course.Id.ToString())).Id;

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
