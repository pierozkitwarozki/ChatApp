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
using Java.Util;

namespace ChatApp.EventListeners
{
    public class FriendAddListener : Java.Lang.Object, Firebase.Firestore.IEventListener
    {
        string currentUserID = FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid;
        string friendId;
        public void InviteFriend(string email)
        {
            FirebaseBackend.FirebaseBackend.GetFireStore()
                .Collection("users")
                .WhereEqualTo("email", email)
                .AddSnapshotListener(this);
        }

        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshotQuery = (QuerySnapshot)value;
            if (!snapshotQuery.IsEmpty)
            {
                foreach (DocumentSnapshot snap in snapshotQuery.Documents)
                {
                    friendId = snap.Id.ToString();
                    DocumentReference documentReference =
                        FirebaseBackend.FirebaseBackend
                        .GetFireStore()
                        .Collection("invitations")
                        .Document(friendId);

                    documentReference.Update(currentUserID, true);
                }
                
            }
                   
        }
    }
}