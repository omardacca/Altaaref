using Altaaref.Models;
using Altaaref.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.NotebooksDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacultyCoursesListPage : ContentPage
    {
        public FacultyCoursesListPage(int facultyId)
        {
            InitializeComponent();

            BindingContext = new FacultyCoursesListViewModel(new PageService(), facultyId);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await (BindingContext as FacultyCoursesListViewModel).CourseSelectedAsync(e.Item as Courses);
        }
    }
}
