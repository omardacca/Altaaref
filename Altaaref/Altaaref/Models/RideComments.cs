using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class RideComments : BaseViewModel
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int RideId { get; set; }
        public Ride Ride { get; set; }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { SetValue(ref _comment, value); }
        }

        public DateTime FullTime { get; set; }
    }
}
