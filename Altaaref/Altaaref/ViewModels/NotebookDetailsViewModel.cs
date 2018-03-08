using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.ViewModels
{
    public class NotebookDetailsViewModel
    {
        public Models.Notebook Notebook { get; private set; } = new Models.Notebook
        {
            Id = 0,
            Name = "Discrete Math Notebook",
            Views = 5,
            CreationDate = new DateTime(2017, 1, 15)
        };

    }
}
