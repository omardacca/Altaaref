using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class FacultyHelpRequest : BaseViewModel
    {
        private StudentHelpRequest _helpRequest;
        public StudentHelpRequest HelpRequest
        {
            get { return _helpRequest; }
            set
            {
                _helpRequest = value;
                OnPropertyChanged(nameof(HelpRequest));
            }
        }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
    }
}
