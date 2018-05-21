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
                    BackgroundImage = "bgnotebookmainmenu.png",
                    IconImage = "",
                    Text = "Notebooks Storage",
                    Column = 0,
                    Row = 0,
                    NavigateType = typeof(NotebooksDB.NotebooksMainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "bgmutualhelpmainmenu.png",
                    IconImage = "",
                    Text = "Mutual Help",
                    Column = 1,
                    Row = 0,
                    NavigateType = typeof(MutualHelp.MainHelpRequest)
                },
                new MenuItem()
                {
                    BackgroundImage = "bgstudygroupmainmenu.png",
                    IconImage = "",
                    Text = "Study Groups",
                    Column = 0,
                    Row = 1,
                    NavigateType = typeof(StudyGroups.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "bghitchhikingmainmenu.png",
                    IconImage = "",
                    Text = "Hitchhiking",
                    Column = 1,
                    Row = 1,
                    NavigateType = typeof(CommonPages.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "bghomepagemainmenu.png",
                    IconImage = "",
                    Text = "Home Page",
                    Column = 0,
                    Row = 2,
                    NavigateType = typeof(CommonPages.MainPage)
                },
                new MenuItem()
                {
                    BackgroundImage = "bgsignoutmainmenu.png",
                    IconImage = "",
                    Text = "Sign Out",
                    Column = 1,
                    Row = 2,
                    NavigateType = typeof(LoginPage)
                }
            };
        }
    }
}
