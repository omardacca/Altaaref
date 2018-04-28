using Altaaref.Models;
using Altaaref.UserControls;
using Altaaref.ViewModels;
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
	public partial class ViewStudyGroupDetails : ContentPage
	{
		public ViewStudyGroupDetails (StudyGroupView studyGroup)
		{
			InitializeComponent ();

            BindingContext = new ViewStudyGroupDetailsViewModel(studyGroup, new PageService());
        }
    }
}