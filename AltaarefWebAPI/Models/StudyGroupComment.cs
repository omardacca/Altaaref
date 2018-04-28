using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class StudyGroupComment
    {
        public int Id { get; set; }

        public int StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string Comment { get; set; }
        public DateTime FullTime { get; set; }
    }
}
