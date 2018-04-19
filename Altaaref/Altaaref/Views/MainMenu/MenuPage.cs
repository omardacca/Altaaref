using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.Views.MainMenu
{
    public class MenuPage
    {
        public Page GetMenuPage()
        {
            return new NavigationPage(new MainMenuPageContent())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.White
            };
        }
    }
}
