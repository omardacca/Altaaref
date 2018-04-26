using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altaaref.UserControls;
using Altaaref.ViewModels;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.StudyGroups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindStudyGroupResults : ContentPage
	{
		public FindStudyGroupResults(int courseid, DateTime from, DateTime to, int numOfAttendants)
		{
			InitializeComponent ();
            BindingContext = new FindStudyGroupResultsViewModel(new PageService(), courseid, from, to, numOfAttendants);
		}

        public FindStudyGroupResults(int courseid, DateTime from, DateTime to)
        {
            InitializeComponent();
            BindingContext = new FindStudyGroupResultsViewModel(new PageService(), courseid, from, to);
        }

        void HandleResultClicked(object sender, ItemTappedEventArgs e)
        {
            var rrv = sender as RoundedRectangleView;
            
            (BindingContext as FindStudyGroupResultsViewModel).StudyGroupResultItemClicked(rrv.CommandParameter as Models.StudyGroup);
        }
    }
}