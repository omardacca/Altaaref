using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class HelpRequestComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }

        public int HelpRequestId { get; set; }
        public HelpRequest HelpRequest { get; set; }
    }
}
