﻿using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindNotebookPage : ContentPage
	{
		public FindNotebookPage ()
		{
			InitializeComponent ();

            BindingContext = new FindNotebookViewModel(new PageService());
        }
	}
}