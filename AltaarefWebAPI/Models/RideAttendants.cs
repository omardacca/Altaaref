using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class RideAttendants
    {
        public int AttendantId { get; set; }
        public Student Attendant { get; set; }

        public int RideId { get; set; }
        public Ride Ride { get; set; }
    }
}
