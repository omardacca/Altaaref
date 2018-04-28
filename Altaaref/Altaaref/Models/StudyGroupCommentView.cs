using System;

namespace Altaaref.Models
{
    public class StudyGroupCommentView
    {
        public int CommentId { get; set; }
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string ProfilePicBlobUrl { get; set; }
        public string Comment { get; set; }
        public DateTime FullTime { get; set; }
    }
}
