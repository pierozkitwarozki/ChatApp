using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ChatApp.EventListeners;
using ChatApp.Fragments;
using Firebase.Auth;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class LoginActivity : AppCompatActivity
    {
        //Views
        EditText emailEditText;
        EditText passwordEditText;
        TextView forgotPasswordTextView;
        TextView signUpTextView;
        Button loginButton;
        ResetPasswordFragment resetPasswordFragment;

        //Firebase
        FirebaseAuth auth;
        TaskComplitionListener listener;
        ResetPasswordListener resetListener;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_login);
            auth = FirebaseBackend.FirebaseBackend.GetFireAuth();
            listener = new TaskComplitionListener();
            ConnectViews();
        }

        private void ConnectViews()
        {
            emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            forgotPasswordTextView = FindViewById<TextView>(Resource.Id.forgotPasswordTextView);
            signUpTextView = FindViewById<TextView>(Resource.Id.signUpTextView);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            signUpTextView.Click += SignUpTextView_Click;
            loginButton.Click += LoginButton_Click;
            forgotPasswordTextView.Click += ForgotPasswordTextView_Click;

        }

        private void ForgotPasswordTextView_Click(object sender, EventArgs e)
        {
            resetListener = new ResetPasswordListener();
            resetPasswordFragment = new ResetPasswordFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            resetPasswordFragment.Cancelable = true;
            resetPasswordFragment.Show(trans, "reset_password");
            resetPasswordFragment.OnPasswordReset += (s, args) =>
            {
                string email = args.Email;
                Android.Support.V7.App.AlertDialog.Builder confirmResetDialog =
               new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
                confirmResetDialog.SetMessage("Reset password?");

                confirmResetDialog.SetNegativeButton("Cancel", (thisalert, args) =>
                {
                    resetPasswordFragment.Dismiss();
                });
                confirmResetDialog.SetPositiveButton("Reset", (thisalert, args) =>
                {
                    resetListener.ResetPassword(auth, email);
                    resetPasswordFragment.Dismiss();
                });
                confirmResetDialog.Show();
            };
            resetListener.listener.Failure += (s, args) =>
            {
                Toast.MakeText(this, "Password failed to reset due to an error: " + args.Cause, ToastLength.Long).Show();
            };
            resetListener.listener.Success += (s, args) =>
            {
                Toast.MakeText(this, "Check your mail box", ToastLength.Short).Show();
            };
            
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Login(emailEditText.Text, passwordEditText.Text);
        }

        private void SignUpTextView_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SignUpActivity));
        }

        private void Login(string email, string password)
        {
            //Login funcionality
            if (!Helpers.Helper.IsValidEmail(email))
            {
                Toast.MakeText(this, "Please provide a valid email", ToastLength.Short).Show();
                return;
            }
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnSuccessListener(listener)
                .AddOnFailureListener(listener);

            //Login failed
            listener.Failure += (s, args) =>
              {
                  Toast.MakeText(this, "Signing up failed due to: " + args.Cause, ToastLength.Short).Show();
              };

            listener.Success += (s, args) =>
            {
                StartActivity(typeof(MainActivity));
                FinishAffinity();
            };

        }
    }
}