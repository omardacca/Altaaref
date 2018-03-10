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

            BindingContext = new NotebookListViewModel();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await this.Navigation.PushAsync(new Views.NotebooksDB.NotebookDetails());

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
