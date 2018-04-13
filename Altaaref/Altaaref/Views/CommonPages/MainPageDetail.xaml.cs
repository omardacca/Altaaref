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
    public partial class MainPageDetail : ContentPage
    {
        public MainPageDetail()
        {
            InitializeComponent();
        }

        async void HandleButtonAddNewStudyGroup(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Views.StudyGroups.NewGroupPage());
        }

        async void HandleButtonFindStudyGroup(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Views.StudyGroups.FindStudyGroup());
        }

        async void HandleButtonNewHelpRequest(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Views.MutualHelp.NewHelpRequest());
        }
    }
}