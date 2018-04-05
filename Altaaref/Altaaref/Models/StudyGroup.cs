using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class StudyGroup
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
//        public Courses Course { get; set; }

        public int StudentId { get; set; }
//        public Student Student { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool IsPublic { get; set; }
    }
}
