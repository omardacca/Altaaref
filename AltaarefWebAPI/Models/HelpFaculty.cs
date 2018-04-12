using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class HelpFaculty
    {
        public int HelpRequestId { get; set; }
        public HelpRequest HelpRequest { get; set; }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
