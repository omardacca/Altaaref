﻿using Altaaref.ViewModels;
using Altaaref.ViewModels.StudyGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.StudyGroups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

            BindingContext = new MainPageViewModel(new PageService());

            //NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}