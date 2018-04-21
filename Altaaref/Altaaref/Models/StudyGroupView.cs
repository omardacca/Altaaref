using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class StudyGroupView
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int NumberOfAttendants { get; set; }
    }
}
