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
    public class Message
    {
        public string ProfileImageId { get; set; }
        public string ProfileName { get; set; }
        public string MessageShort { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}