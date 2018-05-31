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
        public string ProfilePicBlobUrl { get; set; }

        public string IdentityId { get; set; }
        public AppUser Identity { get; set; }

        public ICollection<HelpRequest> HelpRequests { get; set; }
        public ICollection<StudentFavNotebooks> StudentFavNotebooks { get; set; }
        public ICollection<StudyGroup> StudyGroups { get; set; }
        public ICollection<StudyGroupAttendants> StudyGroupAttendants { get; set; }
        public ICollection<StudyGroupInvitations> StudyGroupInvitations { get; set; }
        public ICollection<StudentFaculty> StudentFaculty { get; set; }
        public ICollection<StudentCourses> StudentCourses { get; set; }
        public ICollection<StudyGroupComment> StudyGroupComments { get; set; }
        public ICollection<Notebook> Notebooks { get; set; }
        public ICollection<NotebookRates> NotebookRates { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }
        public ICollection<Ride> Rides { get; set; }

        public ICollection<RideAttendants> RideAttendants { get; set; }
    }
}
