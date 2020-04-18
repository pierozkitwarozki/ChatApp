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
using ChatApp.DataModels;
using ChatApp.EventListeners;
using Newtonsoft.Json;
using Refractored.Controls;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ProfileActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbarProfile;
        _BaseCircleImageView profileCircleImageView;
        ImageView backarrowProfileImageView;
        Button logOutButton;
        TextView fullnameProfileTextView;
        TextView friendsProfileTextView;
        TextView invitationsProfileTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);
            ConnectViews();
        }
        private void SetupToolbar()
        {
            toolbarProfile = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarProfile);
            SetSupportActionBar(toolbarProfile);
        }

        private void ConnectViews()
        {
            SetupToolbar();
            friendsProfileTextView = FindViewById<TextView>(Resource.Id.friendsProfileTextView);
            invitationsProfileTextView = FindViewById<TextView>(Resource.Id.invitationsProfileTextView);
            fullnameProfileTextView = FindViewById<TextView>(Resource.Id.fullnameProfileTextView);
            profileCircleImageView = FindViewById<_BaseCircleImageView>(Resource.Id.profileCircleImageView);
            backarrowProfileImageView = FindViewById<ImageView>(Resource.Id.backarrowProfileImageView);
            invitationsProfileTextView.Click += (s, args) =>
              {
                  StartActivity(typeof(InvitationsActivity));
              };
            logOutButton = FindViewById<Button>(Resource.Id.logOutButton);
            Helpers.Helper.GetCircleImage(Helpers.Helper.GetImageUrl(), profileCircleImageView);
            fullnameProfileTextView.Text = Helpers.Helper.GetFullName();
            backarrowProfileImageView.Click += (s, args) =>
            {
                StartActivity(typeof(MainActivity));
                Finish();
            };
            logOutButton.Click += (s, args) =>
            {
                //MessagesListener listener = new MessagesListener();
                //listener.RemoveListener();
                FirebaseBackend.FirebaseBackend
                  .GetFireAuth()
                  .SignOut();
                Helpers.Helper.ClearISharedPrefernces();
                StartActivity(typeof(LoginActivity));
                FinishAffinity();
            };
            friendsProfileTextView.Click += (s, args) =>
            {               
                StartActivity(typeof(FirendsActivity));
            };
        }

    }
}