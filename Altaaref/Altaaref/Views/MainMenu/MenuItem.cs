using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Views.MainMenu
{
    public class MenuItem
    {
        public string IconImage { get; set; }

        public string BackgroundImage { get; set; }

        public string Text { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public Type NavigateType { get; set; }
    }
}
