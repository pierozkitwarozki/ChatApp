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

namespace ChatApp.DataModels
{
    public class User
    {
        public string User_Id { get; set; }
        public string Fullname { get; set; }
        public string Image_Url { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}