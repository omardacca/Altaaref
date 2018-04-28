using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class MiniStudentView
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ProfilePicBlobUrl { get; set; }
    }
}
