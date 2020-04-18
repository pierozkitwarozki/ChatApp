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
using ChatApp.Activities;
using Firebase.Firestore;
using Java.Util;

namespace ChatApp.EventListeners
{
    public class FriendAddListener : Java.Lang.Object, Firebase.Firestore.IEventListener, IOnSuccessListener, IOnFailureListener
    {
        //This class is responsible for sending invitations to user with entered emails
        string currentUserID = FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid;
        string friendId;

        public event EventHandler<ResultEventArgs> OnResult;
        public class ResultEventArgs: EventArgs
        {
            public string Result { get; set; }
        }
        public void InviteFriend(FirendsActivity friends, string email)
        {
            FirebaseBackend.FirebaseBackend.GetFireStore()
                .Collection("users")
                .WhereEqualTo("email", email)
                .AddSnapshotListener(friends, this);
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

                    documentReference.Update(currentUserID, true)
                        .AddOnSuccessListener(this)
                        .AddOnFailureListener(this);
                }

            }
            else
            {
                OnResult?.Invoke(this, new ResultEventArgs
                {
                    Result = "No user with such email"
                });
            }
                   
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            OnResult?.Invoke(this, new ResultEventArgs
            {
                Result = "Inviting friend failed to an error: " + e.Message
            });
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            OnResult?.Invoke(this, new ResultEventArgs
            {
                Result = "Invitation sent"
            });
        }
    }
}