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
        public string BlobURL { get; set; }
        
        public ICollection<StudentFavNotebooks> StudentFavNotebooks { get; set; }
        public ICollection<NotebookRates> NotebookRates { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

    }
}
