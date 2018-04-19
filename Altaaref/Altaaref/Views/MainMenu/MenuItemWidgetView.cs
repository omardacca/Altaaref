using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Altaaref.Views.MainMenu
{
	public class MenuItemWidgetView : ContentView
	{
        public event EventHandler<WidgetTappedEventArgs> Tapped;

        public MenuItemWidgetView (MenuItem square)
		{
            RelativeLayout layout = new RelativeLayout();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
                Tapped?.Invoke(this, new WidgetTappedEventArgs(square.NavigateType));

            layout.GestureRecognizers.Add(tapGestureRecognizer);

            var backgroundImage = new Image()
            {
                Source = new FileImageSource() { File = square.BackgroundImage },
                Aspect = Aspect.AspectFill,
                InputTransparent = false
            };

            layout.Children.Add(backgroundImage,
            Constraint.Constant(0),
            Constraint.Constant(0),
            Constraint.RelativeToParent((parent) => {
                return parent.Width;
            }),
            Constraint.RelativeToParent((parent) => {
                return parent.Height;
            }));

            var iconImage = new Image()
            {
                Source = new FileImageSource() { File = square.IconImage },
                InputTransparent = true
            };

            layout.Children.Add(
                iconImage,
                Constraint.RelativeToParent((parent) => {
                    return ((parent.Width / 2) - (iconImage.Width / 2));
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height * .25;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width * .45;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width * .45;
                })
            );

            iconImage.SizeChanged += (sender, e) => {
                layout.ForceLayout();
            };

            var dashlabel = new Label()
            {
                Text = square.Text,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
//                FontFamily = Device.OnPlatform("AvenirNextCondensed-Bold", "sans-serif-condensed", null),
                InputTransparent = true
            };

            layout.Children.Add(dashlabel,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height - 30;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height;
                }));

            Content = layout;

        }
	}
}