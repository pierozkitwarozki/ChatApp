using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ChatApp.Fragments
{
    public class ResetPasswordFragment : Android.Support.V4.App.DialogFragment
    {
        EditText emailForgotEditText;
        Button resetPasswordButton;

        public event EventHandler<ResetPasswordEventArgs> OnPasswordReset;

        public class ResetPasswordEventArgs: EventArgs
        {
            public string Email { get; set; }
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.forgot_password, container, false);
            emailForgotEditText = view.FindViewById<EditText>(Resource.Id.emailForgotEditText);
            resetPasswordButton = view.FindViewById<Button>(Resource.Id.resetPasswordButton);
            resetPasswordButton.Click += (s, args) =>
            {
                OnPasswordReset?.Invoke(this, new ResetPasswordEventArgs
                {
                    Email = emailForgotEditText.Text
                });
                emailForgotEditText.Text = "";                
            };
            return view;
        }
    }
}