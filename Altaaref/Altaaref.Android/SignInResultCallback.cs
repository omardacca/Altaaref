﻿using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Java.Lang;

namespace Altaaref.Droid
{
    public class SignInResultCallback : Object, IResultCallback
    {
        public MainActivity Activity { get; set; }

        public void OnResult(Object result)
        {
            var googleSignInResult = result as GoogleSignInResult;
            //Activity.HideProgressDialog();
            Activity.HandleSignInResult(googleSignInResult);
        }
    }
}