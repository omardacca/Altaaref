using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }

        public ICollection<StudentFavNotebooks> StudentFavNotebooks { get; set; }
        public ICollection<StudyGroup> StudyGroups { get; set; }
    }
}
