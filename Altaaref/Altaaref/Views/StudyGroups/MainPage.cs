using Altaaref.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.Views.StudyGroups
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {

            BindingContext = new MyStudyGroupsViewModel(new PageService());

            BackgroundColor = Color.White;

            var backgroundImage = new Image()
            {
                Source = new FileImageSource() { File = "studystudents.png" },
                Aspect = Aspect.AspectFill,
                IsOpaque = true,
                Opacity = 0.8,
            };

            var shader = new BoxView()
            {
                Color = Color.Black.MultiplyAlpha(.5),
            };

            var dome = new Image()
            {
                Aspect = Aspect.AspectFill,
                Source = new FileImageSource() { File = "dome.png" }
            };

            var label = new Label()
            {
                Text = "Study Groups",
                FontSize = 18
            };

            var addimage = new Image()
            {
                Aspect = Aspect.AspectFill,
                Source = new FileImageSource() { File = "addmanicon.png" },
                HeightRequest = 45,
                WidthRequest = 45,
            };

            var addframe = new Frame
            {
                CornerRadius = 1,
                HasShadow = true,
                Content = addimage
            };

            var searchimage = new Image()
            {
                Aspect = Aspect.AspectFill,
                Source = new FileImageSource() { File = "searchicon.png" },
                HeightRequest = 45,
                WidthRequest = 45
            };

            TapGestureRecognizer searchTap = new TapGestureRecognizer();
            searchTap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Views.CommonPages.MainPage());
            };

            searchimage.GestureRecognizers.Add(searchTap);

            var searchframe = new Frame
            {
                CornerRadius = 1,
                HasShadow = true,
                Content = searchimage
            };

            TapGestureRecognizer addTap = new TapGestureRecognizer();
            addTap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Views.CommonPages.MainPage());
            };


            var xscrol = new UserControls.XScrollView()
            {
                HeightRequest = 200,
                ItemTemplate = new DataTemplate(() =>
                {
                    StackLayout slView = new StackLayout();

                    var lb = new Label();
                    lb.SetBinding(Label.TextProperty, "Message");

                    slView.Children.Add(lb);

                    ViewCell vc = new ViewCell() { View = slView };

                    return vc;
                })
            };

            xscrol.SetBinding(UserControls.XScrollView.ItemsSourceProperty, new Binding("BindingContext.StudyGroupList", source: this));


            RelativeLayout relativeLayout = new RelativeLayout()
            {
                HeightRequest = 100,
            };

            relativeLayout.Children.Add(
                backgroundImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height * .5;
                })
            );

            relativeLayout.Children.Add(
                shader,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height * .5;
                })
            );

            relativeLayout.Children.Add(
                dome,
                Constraint.Constant(-10),
                Constraint.RelativeToParent((parent) => {
                    return (parent.Height * .5) - 50;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width + 10;
                }),
                Constraint.Constant(75)
            );

            relativeLayout.Children.Add(
                label,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * .35;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .45);
                })
            );

            relativeLayout.Children.Add(
                addframe,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * .30;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .55);
                })
            );

            relativeLayout.Children.Add(
                searchframe,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * .50;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .55);
                })
            );

            relativeLayout.Children.Add(
                xscrol,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * 0.1;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .70);
                })
            );


            this.Content = relativeLayout;
        }

        private HttpClient _client = new HttpClient();

        private async void GetStudyGroupsByStudentId(int studentId)
        {
            string url = "https://altaarefapp.azurewebsites.net/api/StudyGroups/ById" + studentId;

            string content = await _client.GetStringAsync(url);
            var course = JsonConvert.DeserializeObject<Courses>(content);
            Course = course;
        }
    }
}
