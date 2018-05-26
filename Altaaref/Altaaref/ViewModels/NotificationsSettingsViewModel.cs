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
    public enum NotificationSettingsViewModelType { StudyGroup, MutualHelp, NotebooksStorage }

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

//        private List<string> _namesList;
        //public List<string> NamesList
        //{
        //    get { return _namesList; }
        //    set
        //    {
        //        _namesList = value;
        //        OnPropertyChanged(nameof(NamesList));
        //    }
        //}

        private int _courseId;
        public int CourseId
        {
            get { return _courseId; }
            set { SetValue(ref _courseId, value); }
        }

        public ICommand AddCommand => new Command(async () => { await HandleAddTap(); });

        public NotificationsSettingsViewModel(IPageService pageService, NotificationSettingsViewModelType modelType)
        {
            _pageService = pageService;
            _modelType = modelType;

            string serializedList = Application.Current.Properties["SerializedUserNotif"] as string;
            var list = JsonConvert.DeserializeObject<List<UserNotification>>(serializedList);

            switch(modelType)
            {
                case NotificationSettingsViewModelType.MutualHelp:
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("HRC"));
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("HRF"));
                    break;
                case NotificationSettingsViewModelType.NotebooksStorage:
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("NS"));
                    break;
                case NotificationSettingsViewModelType.StudyGroup:
                    NotificationList = list.FindAll(un => un.StudentId == Settings.StudentId && un.Topic.StartsWith("SG"));
                    break;
            }

        }

        private async Task HandleAddTap()
        {
            switch(_modelType)
            {
                case NotificationSettingsViewModelType.MutualHelp:
                    await HandleHRCourseAddButton();
                    break;
                case NotificationSettingsViewModelType.NotebooksStorage:
                    await HandleNRAddButton();
                    break;
                case NotificationSettingsViewModelType.StudyGroup:
                    await HandleSGAddButton();
                    break;
            }
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


    }
}
