using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FacultyCourse> FacultyCourse { get; set; }
        public ICollection<Notebook> Notebooks { get; set; }
        public ICollection<StudyGroup> StudyGroups { get; set; }
    }
}
