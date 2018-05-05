using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class NotebookRates
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int NotebookId { get; set; }
        public Notebook Notebook { get; set; }

        public byte Rate { get; set; }
    }
}
