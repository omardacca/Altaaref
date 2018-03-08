using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class Notebook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Views { get; set; }
        public DateTime CreationDate { get; set; }
        public Student Author { get; set; }
        public Dictionary<int, int> Rate { get; set; }
    }
}
