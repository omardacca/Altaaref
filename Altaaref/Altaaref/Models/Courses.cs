using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class Courses : StudentStudiesBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Faculty> facultiesList { get; set; }
    }
}
