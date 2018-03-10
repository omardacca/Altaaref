using System;
using System.Collections.Generic;
using System.Text;
using Altaaref.Models;

namespace Altaaref.ViewModels
{
    public class NotebookDetailsViewModel
    {
        public Notebook Notebook { get; private set; } = new Notebook
        {
            Id = 0,
            Name = "Discrete Math Notebook",
            Views = 5,
            CreationDate = new DateTime(2017, 1, 15)
        };

        public NotebookDetailsViewModel(int notebookId)
        {

        }

    }
}
