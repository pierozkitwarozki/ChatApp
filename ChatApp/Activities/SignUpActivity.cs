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
using Firebase.Auth;
using Firebase.Firestore;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SignUpActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbar;
        EditText fullnameSignUpEditText;
        EditText emailSignUpEditText;
        EditText usernameSignUpEditText;
        EditText passwordSignUpEditText;
        EditText confirmPasswordSignUpEditText;
        Button signUpButton;
        ImageView backarrowImageView;

        //Firebase
        FirebaseFirestore database;
        FirebaseAuth auth;
        TaskComplitionListener listener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup);
            auth = FirebaseBackend.FirebaseBackend.GetFireAuth();
            listener = new TaskComplitionListener();
            SetupToolbar();
            ConnectViews();
            // Create your application here
        }

        private void ConnectViews()
        {
            fullnameSignUpEditText = FindViewById<EditText>(Resource.Id.fullnameSignUpEditText);
            emailSignUpEditText = FindViewById<EditText>(Resource.Id.emailSignUpEditText);
            usernameSignUpEditText = FindViewById<EditText>(Resource.Id.usernameSignUpEditText);
            passwordSignUpEditText = FindViewById<EditText>(Resource.Id.passwordSignUpEditText);
            confirmPasswordSignUpEditText = FindViewById<EditText>(Resource.Id.confirmPasswordSignUpEditText);
            signUpButton = FindViewById<Button>(Resource.Id.signUpButton);
            backarrowImageView = FindViewById<ImageView>(Resource.Id.backarrowImageView);
            signUpButton.Click += SignUpButton_Click;
            backarrowImageView.Click += BackarrowImageView_Click;
        }

        private void BackarrowImageView_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        private void SetupToolbar()
        {
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            SignUp(fullnameSignUpEditText.Text,
                emailSignUpEditText.Text,
                usernameSignUpEditText.Text,
                passwordSignUpEditText.Text,
                confirmPasswordSignUpEditText.Text,
                "");
        }

        private void SignUp(string fullname, string email, 
            string username, string password, string confirmPassword, string image)
        {
            //Register funcionality
            if (!Helpers.Helper.IsValidEmail(email))
            {
                Toast.MakeText(this, "Please provide a valid email", ToastLength.Short).Show();
                return;
            }
            else if (password != confirmPassword)
            {
                Toast.MakeText(this, "Passwords does not match", ToastLength.Short).Show();
                return;
            }
            auth.CreateUserWithEmailAndPassword(email, password).
                AddOnSuccessListener(listener)
                .AddOnFailureListener(listener);

            //Register failed
            listener.Failure += (s, args) =>
              {
                  Toast.MakeText(this, "Signing up failed due to: " + args.Cause, ToastLength.Short).Show();
              };
            listener.Success += (s, args) =>
            {
                Toast.MakeText(this, "Welcome to our community " + fullname + " :)", ToastLength.Short).Show();
                StartActivity(typeof(LoginActivity));
                Finish();
            };

        }
    }
}