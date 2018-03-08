using Altaaref.Models;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Altaaref.ViewModels
{
    public class NotebookListViewModel
    {
        public ObservableCollection<Notebook> NotebookList { get; private set; } = new ObservableCollection<Notebook>
        {
            new Notebook { Id = 0, Name = "Intro to CS" },
            new Notebook { Id = 1, Name = "Basics Of Optimetric" },
            new Notebook { Id = 2, Name = "Intro to Marketing" }

        };
    }
}
