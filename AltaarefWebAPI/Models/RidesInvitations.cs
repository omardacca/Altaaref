using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class RidesInvitations
    {
        public Student Candidate { get; set; }
        public int CandidateId { get; set; }

        public Ride Ride { get; set; }
        public int RideId { get; set; }

        public bool Status { get; set; }
    }
}
