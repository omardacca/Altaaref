using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName{ get; set; }
        public DateTime DOB { get; set; }
        public string ProfilePicBlobUrl { get; set; }
    }
}
