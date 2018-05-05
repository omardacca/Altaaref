﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Models
{
    public class Notebook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ViewsCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string BlobURL { get; set; }
        public string FileName { get; set; }

        // for simplicity, currently I will not add FileExtension Column,
        // will updated it after decide which file types will support and how it will be received in the form..
        // public string FileExtension { get; set; } 

        public int CourseId { get; set; }
        public int StudentId { get; set; }
    }
}
