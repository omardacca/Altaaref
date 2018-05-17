using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.UserControls.AccordionView
{
    public class AccordionView : ScrollView
    {
        private StackLayout _layout = new StackLayout { Spacing = 1 };

        public DataTemplate Template { get; set; }
        public DataTemplate SubTemplate { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                propertyName: "ItemsSource",
                defaultBindingMode: BindingMode.TwoWay,
                returnType: typeof(IList),
                declaringType: typeof(AccordionSectionView),
                defaultValue: default(IList),
                propertyChanged: AccordionView.PopulateList);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public AccordionView(DataTemplate itemTemplate)
        {
            this.SubTemplate = itemTemplate;
            this.Template = new DataTemplate(() => (object)(new AccordionSectionView(itemTemplate, this)));
            this.Content = _layout;
        }

        void PopulateList()
        {
            _layout.Children.Clear();

            foreach (object item in this.ItemsSource)
            {
                var template = (View)this.Template.CreateContent();
                template.BindingContext = item;
                _layout.Children.Add(template);
            }
        }

        static void PopulateList(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionView)bindable).PopulateList();
        }
    }


}
