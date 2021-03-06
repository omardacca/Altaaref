﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class RideComments
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int RideId { get; set; }
        public Ride Ride { get; set; }

        public string Comment { get; set; }
        public DateTime FullTime { get; set; }
    }
}
