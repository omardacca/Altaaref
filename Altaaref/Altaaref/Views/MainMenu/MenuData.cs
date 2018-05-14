using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Views.MainMenu
{
    public static class MenuData
    {
        public static List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>()
            {
                new MenuItem()
                {
                    BackgroundImage = "temp1.png",
                    IconImage = "notebookicon.png",
                    Text = "Notebooks Storage",
                    Column = 0,
                    Row = 0,
                    NavigateType = typeof(NotebooksDB.NotebooksMainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "temp2.png",
                    IconImage = "helpicon.png",
                    Text = "Mutual Help",
                    Column = 1,
                    Row = 0,
                    NavigateType = typeof(CommonPages.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "temp3.png",
                    IconImage = "groupicon.png",
                    Text = "Study Groups",
                    Column = 0,
                    Row = 1,
                    NavigateType = typeof(StudyGroups.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "temp4.png",
                    IconImage = "trimicon.png",
                    Text = "Hitchhiking",
                    Column = 1,
                    Row = 1,
                    NavigateType = typeof(CommonPages.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "temp5.png",
                    IconImage = "trimicon.png",
                    Text = "Home Page",
                    Column = 0,
                    Row = 2,
                    NavigateType = typeof(CommonPages.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "temp6.png",
                    IconImage = "trimicon.png",
                    Text = "Sign Out",
                    Column = 1,
                    Row = 2,
                    NavigateType = typeof(LoginPage)
                }
            };
        }
    }
}
