using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class StudyGroupComment
    {
        public int Id { get; set; }
        public int StudyGroupId { get; set; }
        public int StudentId { get; set; }
        public string Comment { get; set; }
        public DateTime FullTime { get; set; }
    }
}
