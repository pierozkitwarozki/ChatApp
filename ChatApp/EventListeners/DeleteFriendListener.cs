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
    public class DeleteFriendListener : Java.Lang.Object, IOnSuccessListener
    {

        private string userId;
        public void DeleteUser(string _userId)
        {
            userId = _userId;

            FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("friends")
                .Document(Helpers.Helper.GetUserId())
                .Get()
                .AddOnSuccessListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            if (snapshot.Exists())
            {
                DocumentReference documentReference = FirebaseBackend.FirebaseBackend
                    .GetFireStore()
                    .Collection("friends")
                    .Document(Helpers.Helper.GetUserId());
                documentReference.Update(userId, FieldValue.Delete());

                DocumentReference documentReference2 = FirebaseBackend.FirebaseBackend
                    .GetFireStore()
                    .Collection("friends")
                    .Document(userId);
                documentReference2.Update(Helpers.Helper.GetUserId(), FieldValue.Delete());
            }
        }
    }
}