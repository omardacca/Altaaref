using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class Notebook : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ViewsCount { get; set; }

        public DateTime PublishDate { get; set; }
        public string BlobURL { get; set; }

        public bool IsPrivate { get; set; }

        public int CourseId { get; set; }
        public Courses Course { get; set; }
        public int StudentId { get; set; }
    }
}
