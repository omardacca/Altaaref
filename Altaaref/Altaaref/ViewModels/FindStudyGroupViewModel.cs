using Altaaref.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public class FindStudyGroupViewModel : BaseViewModel
    {
        int StudentId = 204228043;
        private HttpClient _client = new HttpClient();
        private readonly IPageService _pageService;


        private List<Courses> _itemslist;
        public List<Courses> ItemsList
        {
            get
            {
                return _itemslist;
            }
            set
            {
                _itemslist = value;
                OnPropertyChanged(nameof(ItemsList));
            }
        }

        public List<int> NumberOfAttendantsList { get; set; } = new List<int>();

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                SetValue(ref _fromDate, value);
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                SetValue(ref _toDate, value);
            }
        }


        private bool _datePickersEnabled;
        public bool DatePickersEnabled
        {
            get { return _datePickersEnabled; }
            set
            {
                SetValue(ref _datePickersEnabled, value);
                UpdateFormValidity();
            }
        }

        private void AnyDateOptionChecked()
        {
            if (TodayChecked || TomorrowChecked || ThisweekChecked || NextWeekChecked)
            {
                if (DatePickersEnabled)
                    DatePickersEnabled = false; ;
            }
            else if (!DatePickersEnabled)
                DatePickersEnabled = true;
        }

        private bool _todayChecked;
        public bool TodayChecked
        {
            get { return _todayChecked; }
            set
            {
                SetValue(ref _todayChecked, value);
                AnyDateOptionChecked();
                UpdateFormValidity();
                if (_todayChecked)
                {
                    TomorrowChecked = false;
                    ThisweekChecked = false;
                    NextWeekChecked = false;
                }
            }
        }

        private bool _tomorrowChecked;
        public bool TomorrowChecked
        {
            get { return _tomorrowChecked; }
            set
            {
                SetValue(ref _tomorrowChecked, value);
                AnyDateOptionChecked();
                UpdateFormValidity();
                if(_tomorrowChecked)
                {
                    TodayChecked = false;
                    ThisweekChecked = false;
                    NextWeekChecked = false;
                }
            }
        }

        private bool _thisweekchecked;
        public bool ThisweekChecked
        {
            get { return _thisweekchecked; }
            set
            {
                SetValue(ref _thisweekchecked, value);
                AnyDateOptionChecked();
                UpdateFormValidity();
                if(_thisweekchecked)
                {
                    TodayChecked = false;
                    TomorrowChecked = false;
                    NextWeekChecked = false;
                }
            }
        }

        private bool _nextWeekChecked;
        public bool NextWeekChecked
        {
            get { return _nextWeekChecked; }
            set
            {
                SetValue(ref _nextWeekChecked, value);
                AnyDateOptionChecked();
                UpdateFormValidity();
                if(_nextWeekChecked)
                {
                    TodayChecked = false;
                    TomorrowChecked = false;
                    ThisweekChecked = false;
                }
            }
        }

        private bool _isFormValid;
        public bool IsFormValid
        {
            get { return _isFormValid; }
            set
            {
                SetValue(ref _isFormValid, value);
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

        private int _selectedItemIndex;
        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                SetValue(ref _selectedItemIndex, value);
                UpdateFormValidity();
            }
        }

        private int _selectednumberofattendants;
        public int SelectedNumberOfAttendants
        {
            get { return _selectednumberofattendants; }
            set { SetValue(ref _selectednumberofattendants, value); }
        }

        public ICommand HandleSubmitFind { get; private set; }

        public FindStudyGroupViewModel(IPageService pageService)
        {
            _pageService = pageService;
            InitAsync();

            foreach (int number in Enumerable.Range(0, 20)) NumberOfAttendantsList.Add(number);

            TodayChecked = false;
            TomorrowChecked = false;
            ThisweekChecked = false;
            NextWeekChecked = false;

            DatePickersEnabled = true;

            IsFormValid = false;

            FromDate = DateTime.Now.Date.Date;
            ToDate = DateTime.Now.Date.Date;
        }

        async void InitAsync()
        {
            HandleSubmitFind = new Command(OnHandleFindSubmitButtonTapped);
            await GetCoursesAsync();

        }

        private void OnHandleFindSubmitButtonTapped(object obj)
        {
            var itemid = _itemslist[_selectedItemIndex].Id;
            var numOfAttends = NumberOfAttendantsList[_selectednumberofattendants];
            DateTime from = FromDate, to = ToDate;

                if(TodayChecked)
                {
                    from = DateTime.Now.Date.Date;
                    to = DateTime.Now.Date.Date;
                }
                else if(TomorrowChecked)
                {
                    from = DateTime.Now.Date.Date;
                    to = DateTime.Now.Date.Date.AddDays(1);
                }
                else if(ThisweekChecked)
                {
                    from = DateTime.Now.Date.Date;
                    to = DateTime.Now.Date.Date.AddDays(7 - (int)DateTime.Now.Date.Date.DayOfWeek);
                }
                else if(NextWeekChecked)
                {
                    from = DateTime.Now.Date.Date.AddDays(8 - (int)DateTime.Now.Date.Date.DayOfWeek);
                    to = from.AddDays(7);
                }

                if(numOfAttends > 0)
                    _pageService.PushAsync(new Views.StudyGroups.FindStudyGroupResults(itemid, from, to, numOfAttends));
                else
                    _pageService.PushAsync(new Views.StudyGroups.FindStudyGroupResults(itemid, from, to));
            
        }

        // SHOULD BE: STUDENT COURSES not ALL COURSES
        private async Task GetCoursesAsync()
        {
            Busy = true;
            string url = "https://altaarefapp.azurewebsites.net/api/Courses";

            string content = await _client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<Courses>>(content);
            ItemsList = new List<Courses>(list);

            Busy = false;
        }
        
        private void UpdateFormValidity()
        {
            if (_selectedItemIndex <= 0)
                IsFormValid = false;
            else
                IsFormValid = true;
        }
    }
}
