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
using Firebase.Auth;

namespace ChatApp.EventListeners
{
    public class ResetPasswordListener
    {

        public TaskComplitionListener listener = new TaskComplitionListener();

        public void ResetPassword(FirebaseAuth auth, string email)
        {
            auth.SendPasswordResetEmail(email)
                .AddOnSuccessListener(listener)
                .AddOnFailureListener(listener);
        }
    }
}