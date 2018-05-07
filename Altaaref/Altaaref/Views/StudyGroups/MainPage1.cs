using Altaaref.Models;
using Altaaref.ViewModels;
using Altaaref.ViewModels.StudyGroup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Altaaref.Views.StudyGroups
{
    public class MainPage1 : ContentPage
    {
        /*
        public MainPage1()
        {
            BindingContext = new MainPageViewModel(new PageService());

//            NavigationPage.SetHasNavigationBar(this, false);
//            BackgroundColor = Color.White;

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

            var studygrouplabel = new Label()
            {
                Text = "Study Groups",
                FontSize = 26
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

            addimage.GestureRecognizers.Add(addTap);

            var listtitlelabel = new Label
            {
                Text = "List of study groups in your faculties",
                FontSize = 20
            };

            var xscrol = new UserControls.HorizontalList()
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    StackLayout slView = new StackLayout() { Margin = 5 };
                    StackLayout firstrowsl = new StackLayout() { Orientation = StackOrientation.Horizontal };

                    var coursenamelabel = new Label();
                    coursenamelabel.SetBinding(Label.TextProperty, "CourseName");

                    var studentnamelabel = new Label();
                    studentnamelabel.SetBinding(Label.TextProperty, "StudentName");

                    var messagelabel = new Label();
                    messagelabel.SetBinding(Label.TextProperty, "Message");

                    var datelabel = new Label();
                    datelabel.SetBinding(Label.TextProperty, "Date", stringFormat: "{0:dd/MM/yyyy}");

                    var profielpic = new ImageCircle.Forms.Plugin.Abstractions.CircleImage();
                    profielpic.Aspect = Aspect.AspectFill;
                    profielpic.WidthRequest = 50;
                    profielpic.HeightRequest = 50;
                    profielpic.SetBinding(ImageCircle.Forms.Plugin.Abstractions.CircleImage.SourceProperty, "ProfilePicBlobUrl");

                    firstrowsl.Children.Add(profielpic);
                    firstrowsl.Children.Add(coursenamelabel);
                    firstrowsl.Children.Add(datelabel);

                    slView.Children.Add(firstrowsl);
                    slView.Children.Add(studentnamelabel);
                    slView.Children.Add(messagelabel);

                    ContentView cview = new ContentView() { Content = slView };
                    return cview;
                })
            };

            xscrol.SetBinding(UserControls.HorizontalList.ItemsSourceProperty, new Binding("BindingContext.StudyGroupsList", source: this));
            xscrol.ListOrientation = StackOrientation.Horizontal;

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
                studygrouplabel,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * .32;
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
                    return parent.Width * .32;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .50);
                })
            );

            relativeLayout.Children.Add(
                searchframe,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * .52;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .50);
                })
            );

            relativeLayout.Children.Add(
                listtitlelabel,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * .03;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .65);
                })
            );

            relativeLayout.Children.Add(
                xscrol,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * 0.01;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height * .70);
                })
            );


            this.Content = relativeLayout;
        }

*/
    }
}
