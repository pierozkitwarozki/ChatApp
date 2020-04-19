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
    public class SettingsActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbarSettings;
        ImageView backarrowSettingsImageView;
        Button exitButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);
            ConnectViews();
        }
        private void SetupToolbar()
        {
            toolbarSettings = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarSettings);
            SetSupportActionBar(toolbarSettings);
        }

        private void ConnectViews()
        {
            SetupToolbar();
            backarrowSettingsImageView = FindViewById<ImageView>(Resource.Id.backarrowSettingsImageView);
            exitButton = FindViewById<Button>(Resource.Id.exitButton);
            backarrowSettingsImageView.Click += (s, args) =>
            {
                base.OnBackPressed();
            };
            exitButton.Click += (s, args) =>
            {
                base.OnBackPressed();
            };
        }
    }
}