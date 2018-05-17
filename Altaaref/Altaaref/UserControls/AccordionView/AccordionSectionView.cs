using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.UserControls.AccordionView
{
    public class AccordionSectionView : StackLayout
    {
        private bool _isExpanded = false;
        private StackLayout _content = new StackLayout { HeightRequest = 0 };
        private Color _headerColor = Color.FromHex("0067B7");
        private ImageSource _arrowRight = ImageSource.FromFile("ic_keyboard_arrow_right_white_24dp.png");
        private ImageSource _arrowDown = ImageSource.FromFile("ic_keyboard_arrow_down_white_24dp.png");
        private AbsoluteLayout _header = new AbsoluteLayout();
        private Image _headerIcon = new Image { VerticalOptions = LayoutOptions.Center };
        private Label _headerText = new Label { TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 50 };
        private DataTemplate _template;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                propertyName: "ItemsSource",
                defaultBindingMode: BindingMode.TwoWay,
                returnType: typeof(IList),
                declaringType: typeof(AccordionSectionView),
                defaultValue: default(IList),
                propertyChanged: AccordionSectionView.PopulateList);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                propertyName: "Text",
                returnType: typeof(string),
                declaringType: typeof(AccordionSectionView),
                propertyChanged: AccordionSectionView.ChangeText);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public AccordionSectionView(DataTemplate itemTemplate, ScrollView parent)
        {
            _template = itemTemplate;
            _headerText.BackgroundColor = _headerColor;
            _headerIcon.Source = _arrowRight;
            _header.BackgroundColor = _headerColor;

            _header.Children.Add(_headerIcon, new Rectangle(0, 1, .1, 1), AbsoluteLayoutFlags.All);
            _header.Children.Add(_headerText, new Rectangle(1, 1, .9, 1), AbsoluteLayoutFlags.All);

            this.Spacing = 0;
            this.Children.Add(_header);
            this.Children.Add(_content);

            _header.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        if (_isExpanded)
                        {
                            _headerIcon.Source = _arrowRight;
                            _content.HeightRequest = 0;
                            _content.IsVisible = false;
                            _isExpanded = false;
                        }
                        else
                        {
                            _headerIcon.Source = _arrowDown;
                            _content.HeightRequest = _content.Children.Count * 50;
                            _content.IsVisible = true;
                            _isExpanded = true;

                            // Scroll top by the current Y position of the section
                            if (parent.Parent is VisualElement)
                            {
                                await parent.ScrollToAsync(0, this.Y, true);
                            }
                        }
                    })
                }
            );
        }

        void ChangeText()
        {
            _headerText.Text = this.Text;
        }

        void PopulateList()
        {
            _content.Children.Clear();

            foreach (object item in this.ItemsSource)
            {
                var template = (View)_template.CreateContent();
                template.BindingContext = item;
                _content.Children.Add(template);
            }
        }

        static void ChangeText(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionSectionView)bindable).ChangeText();
        }

        static void PopulateList(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue) return;
            ((AccordionSectionView)bindable).PopulateList();
        }
    }
}
