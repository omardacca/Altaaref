using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class StudyGroupInvitations
    {
        public int StudentId { get; set; }
//        public Student Student { get; set; }

        public int StudyGroupId { get; set; }
//        public StudyGroup StudyGroup { get; set; }

        public bool VerificationStatus { get; set; } = false;

    }
}
