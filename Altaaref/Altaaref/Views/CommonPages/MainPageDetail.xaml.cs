using Altaaref.ViewModels;
using FirebaseNet.Messaging;
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

        
        async void HandleMyNotebooks(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Views.CommonPages.MyNotebooksList());
        }

        async void HandleLogin(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Views.LoginPage());
        }

        async void HandleSendNotif(object sender, ItemTappedEventArgs e)
        {
            FCMClient client = new FCMClient("AAAAnO7dP3I:APA91bEfzkmagwS55b1SpnE8YI_Qn8Hks3prHWhtk3x_OTZ6vLyWDpzH8mPMnDkpahGKxU66wuUSWqe0UCvC_Bn6z3tRkSwXKDafhtkZDbmWQt2AjHlz8VbTINN5XqSogzRiFroz58cl");

            var message = new Message()
            {
                To = "/topics/news",
                Notification = new AndroidNotification()
                {
                    Title = "Title",
                    Body = "body",
                }
            };

            var result = await client.SendMessageAsync(message);
            if(result == null)
            {
                Navigation.PushAsync(new Views.LoginPage());
            }
        }

        //bool authenticated = false;
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();

        //    // Refresh items only when authenticated.
        //    if (authenticated == true)
        //    {
        //        // Set syncItems to true in order to synchronize the data
        //        // on startup when running in offline mode.
        //        //await RefreshItems(true, syncItems: false);

        //        // Hide the Sign-in button.
        //        //this.loginButton.IsVisible = false;
        //    }
        //}
    }
}