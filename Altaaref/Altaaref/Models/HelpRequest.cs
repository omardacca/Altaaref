using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class HelpRequest
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsGeneral { get; set; }
        public bool IsMet { get; set; }
        public int Views { get; set; }

        public int StudentId { get; set; }
    }
}
