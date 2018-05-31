using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class RideAttendants
    {
        public int AttendantId { get; set; }
        public Student Attendant { get; set; }

        public int RideId { get; set; }
        public Ride Ride { get; set; }
    }
}
