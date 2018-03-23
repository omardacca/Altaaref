using Android.Gms.Common.Apis;
using Java.Lang;

namespace Altaaref.Droid
{
    public class SignOutResultCallback : Object, IResultCallback
    {
        public MainActivity Activity { get; set; }

        public void OnResult(Object result)
        {
            //Activity.UpdateUI(false);
        }
    }
}