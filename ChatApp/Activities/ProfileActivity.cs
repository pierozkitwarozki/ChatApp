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
using ChatApp.Fragments;
using Newtonsoft.Json;
using Refractored.Controls;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ProfileActivity : AppCompatActivity
    {
        //Views
        Android.Support.V7.Widget.Toolbar toolbarProfile;
        _BaseCircleImageView profileCircleImageView;
        ImageButton backarrowProfileImageView;
        Button logOutButton;
        TextView fullnameProfileTextView;
        TextView friendsProfileTextView;
        TextView invitationsProfileTextView;
        TextView changePasswordProfileTextView;
        TextView deleteAccountProfileTextView;
        ResetPasswordFragment resetPasswordFragment;

        //Listener
        ResetPasswordListener resetListener;

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
            changePasswordProfileTextView = FindViewById<TextView>(Resource.Id.changePasswordProfileTextView);
            deleteAccountProfileTextView = FindViewById<TextView>(Resource.Id.deleteAccountProfileTextView);
            invitationsProfileTextView = FindViewById<TextView>(Resource.Id.invitationsProfileTextView);
            fullnameProfileTextView = FindViewById<TextView>(Resource.Id.fullnameProfileTextView);
            profileCircleImageView = FindViewById<_BaseCircleImageView>(Resource.Id.profileCircleImageView);
            backarrowProfileImageView = FindViewById<ImageButton>(Resource.Id.backarrowProfileImageView);
            logOutButton = FindViewById<Button>(Resource.Id.logOutButton);
            Helpers.Helper.GetCircleImage(Helpers.Helper.GetImageUrl(), profileCircleImageView);
            fullnameProfileTextView.Text = Helpers.Helper.GetFullName();
            invitationsProfileTextView.Click += (s, args) =>
              {
                  StartActivity(typeof(InvitationsActivity));
              };                       
            backarrowProfileImageView.Click += (s, args) =>
            {
                base.OnBackPressed();
            };
            logOutButton.Click += LogOutButton_Click;
            friendsProfileTextView.Click += (s, args) =>
            {               
                StartActivity(typeof(FirendsActivity));
            };
            changePasswordProfileTextView.Click += ChangePasswordProfileTextView_Click;
            deleteAccountProfileTextView.Click += DeleteAccountProfileTextView_Click;
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            Android.Support.V7.App.AlertDialog.Builder confirmResetDialog =
                    new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            confirmResetDialog.SetMessage("Are you sure to log out?");
            confirmResetDialog.SetNegativeButton("Cancel", (thisalert, args) =>
            {
                //Close dialog
            });
            confirmResetDialog.SetPositiveButton("Log out", (thisalert, args) =>
            {
                    FirebaseBackend.FirebaseBackend
                 .GetFireAuth()
                .SignOut();
                    Helpers.Helper.ClearISharedPrefernces();
                    StartActivity(typeof(LoginActivity));
                    FinishAffinity();
            });
            confirmResetDialog.Show();
        }

        private void DeleteAccountProfileTextView_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Feature to be added in future...", ToastLength.Short).Show();
        }

        private void ChangePasswordProfileTextView_Click(object sender, EventArgs e)
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
                    if (email == Helpers.Helper.GetEmail())
                    {
                        resetListener.ResetPassword(FirebaseBackend.FirebaseBackend.GetFireAuth(), email);
                        resetPasswordFragment.Dismiss();
                    }
                    else Toast.MakeText(this, "Please confirm your email", ToastLength.Long).Show();                    
                    
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
    }
}