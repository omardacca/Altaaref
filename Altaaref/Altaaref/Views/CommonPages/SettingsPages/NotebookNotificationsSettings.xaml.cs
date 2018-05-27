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
	public partial class NotebookNotificationsSettings : ContentPage
	{
		public NotebookNotificationsSettings (NotificationSettingsViewModelType modelType)
		{
			InitializeComponent ();

            BindingContext = new ViewModels.NotificationsSettingsViewModel(new PageService(), modelType);
		}
	}
}