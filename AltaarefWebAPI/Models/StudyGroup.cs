using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class StudyGroup
    {
        public int Id { get; set; }
        
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string Message { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<StudyGroupInvitations> StudyGroupInvitations { get; set; }
        public ICollection<StudyGroupAttendants> StudyGroupAttendants { get; set; }
        public ICollection<StudyGroupComment> StudyGroupComments { get; set; }

    }
}
