using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.Templates.Notebooks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewFavoriteNotebooksTemplate : ContentView
	{
		public ViewFavoriteNotebooksTemplate ()
		{
			InitializeComponent ();
		}

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(ViewFavoriteNotebooksTemplate), null, propertyChanged: OnParentContextPropertyChanged);

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as ViewFavoriteNotebooksTemplate).ParentContext = newValue;
            }
        }
    }
}