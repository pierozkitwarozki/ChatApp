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

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ProfileActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbarProfile;
        ImageView backarrowProfileImageView;
        Button logOutButton;

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
            backarrowProfileImageView = FindViewById<ImageView>(Resource.Id.backarrowProfileImageView);
            logOutButton = FindViewById<Button>(Resource.Id.logOutButton);
            backarrowProfileImageView.Click += (s, args) =>
            {
                StartActivity(typeof(MainActivity));
                Finish();
            };
            logOutButton.Click += (s, args) =>
            {
                FirebaseBackend.FirebaseBackend
                  .GetFireAuth()
                  .SignOut();

                StartActivity(typeof(LoginActivity));
                  FinishAffinity();
            };
        }
    }
}