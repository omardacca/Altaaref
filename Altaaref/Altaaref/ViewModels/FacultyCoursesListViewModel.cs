using Altaaref.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Altaaref.ViewModels
{
    public class FacultyCoursesListViewModel
    {
        public ObservableCollection<Courses> CoursesList { get; private set; } = new ObservableCollection<Courses>
            {
                new Courses { Id = 0, Name = "Intro to CS" },
                new Courses { Id = 1, Name = "Discrete Mathmatics" },
                new Courses { Id = 2, Name = "English Letreture A" },
                new Courses { Id = 3, Name = "English Letreture B" }
            };

    }
}
