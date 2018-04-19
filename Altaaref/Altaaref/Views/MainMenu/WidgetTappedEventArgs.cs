using System;
using System.Collections.Generic;
using System.Text;

namespace Altaaref.Views.MainMenu
{
    public class WidgetTappedEventArgs : EventArgs
    {
        private Type _page;
        public Type Page
        {
            get { return _page; }
        }

        public WidgetTappedEventArgs(Type page)
        {
            _page = page;
        }
    }
}
