using Altaaref.ViewModels;
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

        async void HandleButtonMainViewHelpRequest(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Views.MutualHelp.MainHelpRequest());
        }

        async void HandleButtonMainMenuPage(object sender, ItemTappedEventArgs e)
        {
            var page = new Views.MainMenu.MenuPage().GetMenuPage();
            NavigationPage.SetHasNavigationBar(page, false);
            await Navigation.PushAsync(page);
        }

        bool authenticated = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                //await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                //this.loginButton.IsVisible = false;
            }
        }
    }
}