using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChatApp.DataModels;
using Firebase.Firestore;

namespace ChatApp.EventListeners
{
    public class UserDataListener : Java.Lang.Object, IOnSuccessListener
    {
        //This class implements retrieving single user data via event

        public event EventHandler<UserEventArgs> OnUserRetrieved;
        private User thisUser = new User();
        public List<User> users { get; set; } = new List<User>();

        public class UserEventArgs : EventArgs
        {
            public User UserData { get; set; }
        }
       
        public void OnSuccess(Java.Lang.Object result)
        {
            InstantiateUser(result);
        }

        private void InstantiateUser(Java.Lang.Object result)
        {
            DocumentSnapshot snap = (DocumentSnapshot)result;
            if (snap.Exists())
            {
                thisUser = new User
                {
                    Image_Url = snap.Get("image_id") != null ? snap.Get("image_id").ToString() : "",
                    Fullname = snap.Get("fullname").ToString(),
                    User_Id = snap.Id.ToString(),
                    Email = snap.Get("email").ToString()
                };         
                InvokeEvent();
            }
        }

        public void InvokeEvent()
        {
            OnUserRetrieved?.Invoke(this, new UserEventArgs
            {
                UserData = thisUser
            });
        }
    }
}