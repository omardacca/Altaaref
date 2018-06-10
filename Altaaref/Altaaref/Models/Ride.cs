using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Altaaref.Models
{
    public class Ride : BaseViewModel
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

        private byte _numoffreeseats;
        public byte NumOfFreeSeats
        {
            get { return _numoffreeseats; }
            set
            {
                SetValue(ref _numoffreeseats, value);
            }
        }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }

        public int DriverId { get; set; }
        public Student Driver { get; set; }

        public List<RideAttendants> RideAttendants { get;set; }
        public List<RideComments> RideComments { get; set; }

    }
}
