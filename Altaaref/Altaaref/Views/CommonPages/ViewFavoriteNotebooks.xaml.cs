﻿using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewFavoriteNotebooks : ContentPage
	{
		public ViewFavoriteNotebooks ()
		{
			InitializeComponent ();

            BindingContext = new ViewFavoriteNotebooksViewModel(new PageService());
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ViewFavoriteNotebooksViewModel).ViewFavoriteNotebookSelected(e.Item as ViewNotebookStudent);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as ViewFavoriteNotebooksViewModel).GetFavoriteNotebooksList();
        }
    }
}