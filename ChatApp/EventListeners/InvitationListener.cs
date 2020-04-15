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
using Firebase.Firestore;

namespace ChatApp.EventListeners
{
    public class InvitationListener : Java.Lang.Object, IOnSuccessListener
    {
        //This class implements fetching invitations
        string currentUserID = FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid;
        public UserDataListener listener = new UserDataListener();

        public void FetchInvitations()
        {
            FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("invitations")
                .Document(currentUserID)
                .Get()
                .AddOnSuccessListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            DocumentSnapshot snap = (DocumentSnapshot)result;
            Console.WriteLine(snap.Id);
            if (snap.Exists())
            {
                IDictionary<string, Java.Lang.Object> keyValuePairs = snap.Data;
                foreach (KeyValuePair<string, Java.Lang.Object> pair in keyValuePairs)
                {
                    //Console.WriteLine("{0}: {1}", pair.Key.ToString(), pair.Value.ToString());
                    FirebaseBackend.FirebaseBackend
                        .GetFireStore()
                        .Collection("users")
                        .Document(pair.Key.ToString())
                        .Get()
                        .AddOnSuccessListener(listener);
                }
            }
            else Console.WriteLine("Not found");
        }
    }
}