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
using ChatApp.Helpers;
using Firebase.Firestore;

namespace ChatApp.EventListeners
{
    public class FullnameListener : Java.Lang.Object, IOnSuccessListener, IOnFailureListener
    {
        public void FetchUser()
        {
             FirebaseBackend.FirebaseBackend.GetFireStore()
                .Collection("users")
                .Document(FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid)
                .Get()
                .AddOnSuccessListener(this)
                .AddOnFailureListener(this);
        }


        public void OnFailure(Java.Lang.Exception e)
        {
            throw new NotImplementedException();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            DocumentSnapshot snapshot = (DocumentSnapshot)result;
            if (snapshot.Exists())
            {
                string fullname = snapshot.Get("fullname").ToString();
                string username = snapshot.Get("username").ToString();
                string image_url = snapshot.Get("image_id") != null ? snapshot.Get("image_id").ToString() : "";
                Helper.SaveFullname(fullname);
                Helper.SaveUsername(username);
                Helper.SaveImageUrl(image_url);
            }
        }
    }
}