using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class HelpRequest
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsGeneral { get; set; } = false;
        public bool IsMet { get; set; } = false;
        public int Views { get; set; } = 0;

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public ICollection<HelpFaculty> HelpFaculties { get; set; }
        public ICollection<HelpRequestComment> Comments { get; set; }

    }
}
