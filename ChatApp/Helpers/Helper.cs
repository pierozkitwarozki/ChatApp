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
        public static string GenerateRandomString(int length)
        {
            System.Random rand = new System.Random();
            char[] allowchars = "ABCDEFGHIJKLOMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string sResult = "";
            for (int i = 0; i <= length; i++)
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

        public static void SaveEmail(string email)
        {
            editor = preferences.Edit();
            editor.PutString("email", email);
            editor.Apply();
        }
        public static string GetEmail()
        {
            string email = "";
            email = preferences.GetString("email", "");
            return email;
        }

        public static void SaveUserId(string id)
        {
            editor = preferences.Edit();
            editor.PutString("user_id", id);
            editor.Apply();
        }


        public static string GetUserId()
        {
            string id = "";
            id = preferences.GetString("user_id", "");
            return id;
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

        public static string GenerateMessageId()
        {
            System.Random rand = new System.Random();
            int length = rand.Next(10, 23);
            char[] allowchars = "ABCDEFGHIJKLOMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string sResult = "";
            for (int i = 0; i <= length; i++)
            {
                sResult += allowchars[rand.Next(0, allowchars.Length)];
            }

            return sResult;

        }

        public static string GenerateChatId(string id1, string id2)
        {
            List<int> wordOne = new List<int>();
            List<int> wordTwo = new List<int>();
            foreach (char c in id1)
            {
                wordOne.Add(Convert.ToInt32(c));
            }
            foreach (char c in id2)
            {
                wordTwo.Add(Convert.ToInt32(c));
            }
            int sumOne = 0;
            int sumTwo = 0;
            foreach (int i in wordOne)
            {
                sumOne = sumOne + i;
            }
            foreach (int i in wordTwo)
            {
                sumTwo = sumTwo + i;
            }

            double averageOne = (double)sumOne / (double)wordOne.Max();
            double averageTwo = (double)sumTwo / (double)wordTwo.Max();

            if (averageOne < averageTwo)
            {
                return id1 + "_" + id2;
            }
            else return id2 + "_" + id1;
        }

        public static void ClearISharedPrefernces()
        {
            editor.Clear();
            editor.Apply();
        }





    }
}