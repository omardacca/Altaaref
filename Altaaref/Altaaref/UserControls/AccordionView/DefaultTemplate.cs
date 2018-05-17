using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.UserControls.AccordionView
{
    public class DefaultTemplate : AbsoluteLayout
    {
        public DefaultTemplate()
        {
            this.Margin = 2;
            this.HeightRequest = 60;
            this.BackgroundColor = Color.FromHex("#F7F7F7");

            //var stacklayout = new AbsoluteLayout { HorizontalOptions = LayoutOptions.StartAndExpand, BackgroundColor = Color.Yellow };
            var Title = new Label { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand };
            var CheckIcon = new Image {Source="vicon.png", HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Center };
            var UnCheckIcon = new Image {Source="xicon.png", HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Center };


            this.Children.Add(Title, new Rectangle(0, 0.5, 0.5, 1), AbsoluteLayoutFlags.All);
            this.Children.Add(CheckIcon, new Rectangle(1, 0.5, 0.5, 1), AbsoluteLayoutFlags.All);
            this.Children.Add(UnCheckIcon, new Rectangle(1, 0.5, 0.5, 1), AbsoluteLayoutFlags.All);
            //stacklayout.Children.Add(Title);
            //stacklayout.Children.Add(CheckIcon);
            //stacklayout.Children.Add(UnCheckIcon);

            //this.Children.Add(stacklayout, new Rectangle(0, 0.5, 1, 1), AbsoluteLayoutFlags.All);
            Title.SetBinding(Label.TextProperty, "Course.Name");
            CheckIcon.SetBinding(Image.IsVisibleProperty, "IsChecked", converter: new InverseBooleanConverter());
            UnCheckIcon.SetBinding(Image.IsVisibleProperty, "IsChecked");

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            var bind = new Binding(path: "ParentContext.ItemTapCommand", source: this);
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, bind);

            var parbind = new Binding(path: ".");
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, parbind);
            CheckIcon.GestureRecognizers.Add(tapGestureRecognizer);
            UnCheckIcon.GestureRecognizers.Add(tapGestureRecognizer);

        }

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(DefaultTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as DefaultTemplate).ParentContext = newValue;
            }
        }
    }
}
