using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.Views.MainMenu
{
    public class MainMenuPageContent : ContentPage
    {
        public MainMenuPageContent()
        {
            Title = "Menu Page";

            var menu = new Grid()
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Enumerable.Range(0, 3).ToList().ForEach(x =>
             menu.RowDefinitions.Add(
             new RowDefinition
             {
                 Height = new GridLength(1, GridUnitType.Star)
             }
            ));

            Enumerable.Range(0, 2).ToList().ForEach(x =>
             menu.ColumnDefinitions.Add(new ColumnDefinition
             {
                 Width = new GridLength(1, GridUnitType.Star)
             }));

            var squares = MenuData.GetMenuItems();

            squares.ForEach(x => 
            {
                var widget = new MenuItemWidgetView(x);
                widget.Tapped += async(object sender, WidgetTappedEventArgs e) => {

                    if(x.Row == 2 && x.Column == 1)
                    {
                        var result = await DisplayAlert("Verify", "Are you sure you want to logout ?", "Yes", "No");
                        if(result)
                            await Navigation.PushAsync(new Views.LoginPage(LoginPage.LOGOUT_CODE));
                    }
                    else
                    {
                        var page = Activator.CreateInstance(e.Page) as Page;
                        await Navigation.PushAsync(page);
                    }

                };
                menu.Children.Add(widget, x.Column, x.Row);
            });

            Content = menu;
        }
    }
}
