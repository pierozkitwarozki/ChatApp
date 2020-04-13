using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Refractored.Controls;

namespace ChatApp.Helpers
{
    public static class Helper
    {
        static ISharedPreferences preferences = Application
            .Context
            .GetSharedPreferences("userinfo", FileCreationMode.Private);

        static ISharedPreferencesEditor editor;

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string GenerateRandomString(int lenght)
        {
            System.Random rand = new System.Random();
            char[] allowchars = "ABCDEFGHIJKLOMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string sResult = "";
            for (int i = 0; i <= lenght; i++)
            {
                sResult += allowchars[rand.Next(0, allowchars.Length)];
            }

            return sResult;
        }
        public static void SaveFullname(string fullname)
        {
            editor = preferences.Edit();
            editor.PutString("fullname", fullname);
            editor.Apply();
        }

        public static string GetFullName()
        {
            string fullname = "";
            fullname = preferences.GetString("fullname", "");
            return fullname;
        }
        public static void SaveUsername(string username)
        {
            editor = preferences.Edit();
            editor.PutString("username", username);
            editor.Apply();
        }

        public static string GetUsername()
        {
            string username = "";
            username = preferences.GetString("username", "");
            return username;
        }
        public static void SaveImageUrl(string url)
        {
            editor = preferences.Edit();
            editor.PutString("image_url", url);
            editor.Apply();
        }

        public static string GetImageUrl()
        {
            string url = "";
            url = preferences.GetString("image_url", "");
            return url;
        }
        public static void GetCircleImage(string url, _BaseCircleImageView imageView)
        {
            if (url != "")
            {
                ImageService.Instance.LoadUrl(url)
                .Retry(3, 200)
                .DownSample(400, 400)
                .Into(imageView);
            }    
        }
    }
}