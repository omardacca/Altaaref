using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Altaaref.ViewModels
{
    public interface IPageService
    {
        Task PushAsync(Page page);
        Task PopAsync();
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
    }
}
