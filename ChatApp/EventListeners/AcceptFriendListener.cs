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
using Firebase.Firestore;
using Java.Util;

namespace ChatApp.EventListeners
{
    public class AcceptFriendListener : Java.Lang.Object
    {
        string currentUserID = FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid;
        public void AcceptFriend(string friendId)
        {
            DocumentReference documentReference =
                FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("friends")
                .Document(currentUserID);

            IDictionary<string, Java.Lang.Object> updates = new Dictionary<string, Java.Lang.Object>();
            updates.Add(friendId, true);
            documentReference.Update(updates);

            DocumentReference documentReference2 =
                FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("friends")
                .Document(friendId);

            IDictionary<string, Java.Lang.Object> updates2 = new Dictionary<string, Java.Lang.Object>();
            updates2.Add(currentUserID, true);
            documentReference2.Update(updates2);

            DocumentReference documentReference3 =
                FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("invitations")
                .Document(currentUserID);

            IDictionary<string, Java.Lang.Object> updates3 = new Dictionary<string, Java.Lang.Object>();
            updates3.Add(friendId, FieldValue.Delete());
            documentReference3.Update(updates3);

        }

        public void DeleteInvitation(string friendId)
        {
            DocumentReference documentReference3 =
                   FirebaseBackend.FirebaseBackend
                   .GetFireStore()
                   .Collection("invitations")
                   .Document(currentUserID);

            IDictionary<string, Java.Lang.Object> updates3 = new Dictionary<string, Java.Lang.Object>();
            updates3.Add(friendId, FieldValue.Delete());
            documentReference3.Update(updates3);

        }
        
    }
}