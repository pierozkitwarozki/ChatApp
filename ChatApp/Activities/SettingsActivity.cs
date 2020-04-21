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
        ImageButton backarrowSettingsImageView;
        Button exitButton;
        TextView notificationsSettingsTextView;
        TextView privateModeSettingsTextView;
        TextView defaultSettingsTextView;
        TextView pinkModeSettingsTextView;
        TextView saveSettingsTextView;

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
            backarrowSettingsImageView = FindViewById<ImageButton>(Resource.Id.backarrowSettingsImageView);
            exitButton = FindViewById<Button>(Resource.Id.exitButton);
            notificationsSettingsTextView = FindViewById<TextView>(Resource.Id.notificationsSettingsTextView);
            defaultSettingsTextView = FindViewById<TextView>(Resource.Id.defaultSettingsTextView);
            privateModeSettingsTextView = FindViewById<TextView>(Resource.Id.privateModeSettingsTextView);
            pinkModeSettingsTextView = FindViewById<TextView>(Resource.Id.pinkModeSettingsTextView);
            saveSettingsTextView = FindViewById<TextView>(Resource.Id.saveSettingsTextView);

            notificationsSettingsTextView.Click += NotificationsSettingsTextView_Click;
            defaultSettingsTextView.Click += NotificationsSettingsTextView_Click;
            privateModeSettingsTextView.Click += NotificationsSettingsTextView_Click;
            pinkModeSettingsTextView.Click += NotificationsSettingsTextView_Click;
            saveSettingsTextView.Click += NotificationsSettingsTextView_Click;

            backarrowSettingsImageView.Click += (s, args) =>
            {
                base.OnBackPressed();
            };
            exitButton.Click += (s, args) =>
            {
                base.OnBackPressed();
            };
        }

        private void NotificationsSettingsTextView_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Feature to be added in future...", ToastLength.Short).Show();
        }
    }
}