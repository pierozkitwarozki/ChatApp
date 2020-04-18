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
    public class Conversation
    {
        //This class implements conversation preview
        public string ChatId { get; set; }
        public string UserId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string LastMessagePreview { get; set; }
        public DateTime LastMessageDate { get; set; }

    }
}