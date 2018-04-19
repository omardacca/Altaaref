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
                widget.Tapped += (object sender, WidgetTappedEventArgs e) => {
                    var page = Activator.CreateInstance(e.Page) as Page;
                    Navigation.PushAsync(page);
                };
                menu.Children.Add(widget, x.Column, x.Row);
            });

            Content = menu;
        }
    }
}
