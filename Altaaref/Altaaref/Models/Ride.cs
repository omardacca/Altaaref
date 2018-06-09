﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class Ride
    {
        public int Id { get; set; }
        public string FromCity { get; set; }
        public double FromLong { get; set; }
        public double FromLat { get; set; }
        public string ToCity { get; set; }
        public double ToLong { get; set; }
        public double ToLat { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public byte NumOfFreeSeats { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }

        public int DriverId { get; set; }
        public Student Driver { get; set; }

        public List<RideAttendants> RideAttendants { get; set; }
        public List<RideComments> RideComments { get; set; }

    }
}
