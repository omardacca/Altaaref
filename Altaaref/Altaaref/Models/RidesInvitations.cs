using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class RidesInvitations : BaseViewModel
    {
        public Student Candidate { get; set; }
        public int CandidateId { get; set; }

        public Ride Ride { get; set; }
        public int RideId { get; set; }

        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                SetValue(ref _status, value);
            }
        }
    }
}
