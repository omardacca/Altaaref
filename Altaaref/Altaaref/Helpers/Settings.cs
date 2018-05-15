using Altaaref.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace Altaaref.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("Username", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Username", value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }

        public static DateTime AccessTokenExpiration
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessTokenExpiration", DateTime.UtcNow);
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessTokenExpiration", value);
            }
        }

        public static string Identity
        {
            get
            {
                return AppSettings.GetValueOrDefault("Identity", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Identity", value);
            }
        }

        public static int StudentId
        {
            get
            {
                return AppSettings.GetValueOrDefault("StudentId", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("StudentId", value);
            }
        }
    }
}