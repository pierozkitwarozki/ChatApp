using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ChatApp.Adapters;
using ChatApp.DataModels;
using ChatApp.EventListeners;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class InvitationsActivity : AppCompatActivity
    {

        //Controls
        Android.Support.V7.Widget.Toolbar toolbarInvitations;
        ImageButton backarrowInvitationsImageView;
        RecyclerView invitationsListRecyclerView;

        //Adapters
        InvitationAdapter invitationAdapter;

        //Data
        List<User> users;

        //Listeners
        InvitationListener invitationListener;
        AcceptFriendListener acceptFriendListener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_invitations);
            acceptFriendListener = new AcceptFriendListener();
            users = new List<User>();
            GetFriends();
            ConnectViews();
            SetupRecyclerView();
            // Create your application here
        }
       
        private void SetupToolbar()
        {
            toolbarInvitations = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarInvitations);
            SetSupportActionBar(toolbarInvitations);
        }

        private void ConnectViews()
        {
            SetupToolbar();
            invitationsListRecyclerView = FindViewById<RecyclerView>(Resource.Id.invitationsListRecyclerView);
            backarrowInvitationsImageView = FindViewById<ImageButton>(Resource.Id.backarrowInvitationsImageView);
            backarrowInvitationsImageView.Click += (s, args) =>
            {
                base.OnBackPressed();
            };
        }


        private void SetupRecyclerView()
        {

            invitationsListRecyclerView.SetLayoutManager(new LinearLayoutManager(invitationsListRecyclerView.Context));
            invitationAdapter = new InvitationAdapter(users);
            invitationAdapter.NotifyDataSetChanged();
            invitationAdapter.AcceptClick += (s, e) =>
            {
                acceptFriendListener.AcceptFriend(users[e.Position].User_Id);
                Toast.MakeText(this, "You and " + users[e.Position].Fullname + " are now friends!", ToastLength.Short).Show();
                users.RemoveAt(e.Position);               
                invitationAdapter.NotifyDataSetChanged();
            };
            invitationAdapter.DeleteClick += (s, e) =>
            {
                ConfirmYourDecision(e);
            };
            invitationsListRecyclerView.SetAdapter(invitationAdapter);
        }

        private void ConfirmYourDecision(InvitationAdapterClickEventArgs e)
        {
            Android.Support.V7.App.AlertDialog.Builder deleteAlert =
                new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            deleteAlert.SetMessage("Delete invitation from " + users[e.Position].Fullname + "?");

            deleteAlert.SetNegativeButton("Cancel", (thisalert, args) =>
            {
                //close alertDialog (do nothing)
            });
            deleteAlert.SetPositiveButton("Delete", (thisalert, args) =>
            {
                acceptFriendListener.DeleteInvitation(users[e.Position].User_Id);
                users.RemoveAt(e.Position);
                invitationAdapter.NotifyDataSetChanged();
            });
            deleteAlert.Show();
        }

        private void GetFriends()
        {
            invitationListener = new InvitationListener();
            invitationListener.FetchInvitations();
            invitationListener.listener.OnUserRetrieved += Listener_OnUserRetrieved;

        }

        private void Listener_OnUserRetrieved(object sender, UserDataListener.UserEventArgs e)
        {
            if (!users.Contains(e.UserData))
            {
                users.Add(e.UserData);
                if (users.Count > 0)
                {
                    users.OrderBy(x => x.Fullname).ToList();
                }
                SetupRecyclerView();
            }
        }
    }
}