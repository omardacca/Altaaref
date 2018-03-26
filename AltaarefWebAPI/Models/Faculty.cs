using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FacultyCourse> FacultyCourse { get; set; }
        public ICollection<StudentFaculty> StudentFaculty { get; set; }

    }
}
