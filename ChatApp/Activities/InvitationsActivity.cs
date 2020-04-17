using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ChatApp.Adapters;
using ChatApp.DataModels;
using ChatApp.EventListeners;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class InvitationsActivity : Activity
    {

        //Controls
        ImageView backarrowInvitationsImageView;
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

        private void ConnectViews()
        {
            invitationsListRecyclerView = FindViewById<RecyclerView>(Resource.Id.invitationsListRecyclerView);
            backarrowInvitationsImageView = FindViewById<ImageView>(Resource.Id.backarrowInvitationsImageView);
            backarrowInvitationsImageView.Click += (s, args) =>
            {
                StartActivity(typeof(ProfileActivity));
                Finish();
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
                users.RemoveAt(e.Position);
                invitationAdapter.NotifyDataSetChanged();
            };
            invitationAdapter.DeleteClick += (s, e) =>
            {
                acceptFriendListener.DeleteInvitation(users[e.Position].User_Id);
                users.RemoveAt(e.Position);
                invitationAdapter.NotifyDataSetChanged();
            };
            invitationsListRecyclerView.SetAdapter(invitationAdapter);
        }

        private void GetFriends()
        {
            invitationListener = new InvitationListener();
            invitationListener.FetchInvitations();
            invitationListener.listener.OnUserRetrieved += Listener_OnUserRetrieved;

        }

        private void Listener_OnUserRetrieved(object sender, UserDataListener.UserEventArgs e)
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