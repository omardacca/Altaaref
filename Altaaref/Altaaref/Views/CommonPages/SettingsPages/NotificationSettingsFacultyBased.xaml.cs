using Altaaref.Models;
using Altaaref.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views.CommonPages.SettingsPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationSettingsFacultyBased : ContentPage
	{
		public NotificationSettingsFacultyBased (NotificationSettingsViewModelType modelType)
		{
			InitializeComponent ();

            BindingContext = new NotificationFacultyBasedViewModel(new PageService(), modelType);
		}
	}
}