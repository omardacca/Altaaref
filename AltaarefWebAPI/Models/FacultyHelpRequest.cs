using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class FacultyHelpRequest
    {
        public HelpRequest HelpRequest { get; set; }
        public int FacultyId { get; set; }
    }
}
