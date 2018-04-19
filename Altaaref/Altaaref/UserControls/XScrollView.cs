using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.UserControls
{
    public class XScrollView : ScrollView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(XScrollView), default(IEnumerable));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(XScrollView), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public XScrollView()
        {
            this.PropertyChanged += XScrollView_PropertyChanged;
        }

        void XScrollView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource" || e.PropertyName == "ItemTemplate")
            {
                LoadItems();
            }
        }

        void LoadItems()
        {
            if (this.ItemTemplate == null || this.ItemsSource == null)
                return;

            var layout = new StackLayout();
            layout.Orientation = this.Orientation == ScrollOrientation.Vertical
                ? StackOrientation.Vertical : StackOrientation.Horizontal;

            foreach (var item in this.ItemsSource)
            {
                var viewCell = this.ItemTemplate.CreateContent() as ViewCell;
                viewCell.View.BindingContext = item;
                layout.Children.Add(viewCell.View);
            }

            this.Content = layout;
        }
    }
}