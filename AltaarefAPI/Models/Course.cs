using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefAPI
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseFaculty> CourseFaculty { get; set; }
    }
}
