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
    public partial class NotebooksListPage : ContentPage
    {
        public NotebooksListPage(int courseId)
        {
            InitializeComponent();

            BindingContext = new NotebookListViewModel(new PageService(), courseId);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await (BindingContext as NotebookListViewModel).NotebookSelectedAsync(e.Item as Notebook);
        }
    }
}
