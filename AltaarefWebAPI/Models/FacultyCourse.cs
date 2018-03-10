using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class FacultyCourse
    {
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
