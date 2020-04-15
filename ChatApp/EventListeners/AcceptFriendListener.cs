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
        //This class implements what happens after user decides to accept or decline invitation

        string currentUserID = FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid;
        public void AcceptFriend(string friendId)
        {
            DocumentReference documentReference =
                FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("friends")
                .Document(currentUserID);
            documentReference.Update(friendId, true);

            DocumentReference documentReference2 =
                FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("friends")
                .Document(friendId);
            documentReference2.Update(currentUserID, true);

            DocumentReference documentReference3 =
                FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("invitations")
                .Document(currentUserID);

            documentReference3.Update(friendId, FieldValue.Delete());

        }

        public void DeleteInvitation(string friendId)
        {
            DocumentReference documentReference3 =
                   FirebaseBackend.FirebaseBackend
                   .GetFireStore()
                   .Collection("invitations")
                   .Document(currentUserID);
            documentReference3.Update(friendId, FieldValue.Delete());
        }
        
    }
}