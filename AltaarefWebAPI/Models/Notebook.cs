using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Models
{
    public class Notebook
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public int ViewsCount { get; set; }
        public DateTime PublishDate { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
