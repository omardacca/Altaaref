using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class StudentHelpComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public Student Student { get; set; }
    }
}
