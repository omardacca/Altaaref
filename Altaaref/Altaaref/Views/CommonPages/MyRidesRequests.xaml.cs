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
	public partial class MyRidesRequests : ContentPage
	{
		public MyRidesRequests ()
		{
			InitializeComponent ();

            BindingContext = new ViewModels.Hitchhicking.MyRidesRequestsViewModel(new PageService());
		}
	}
}