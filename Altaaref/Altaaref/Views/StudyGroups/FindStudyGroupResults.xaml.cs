using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altaaref.ViewModels;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.StudyGroups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindStudyGroupResults : ContentPage
	{
		public FindStudyGroupResults (Models.StudyGroup studyGroup)
		{
			InitializeComponent ();
            BindingContext = new FindStudyGroupResultsViewModel(studyGroup, new PageService());
		}
	}
}