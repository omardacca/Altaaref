using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefAPI
{
    public class CourseFaculty
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
