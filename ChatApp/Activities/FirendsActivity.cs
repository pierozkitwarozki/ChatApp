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
using ChatApp.Fragments;
using Newtonsoft.Json;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class FirendsActivity : AppCompatActivity
    {
        //Controls
        RecyclerView friendListRecyclerView;
        FriendsListAdapter friendsListAdapter;
        ImageView backarrowFriendsImageView;
        ImageView addFriendsImageView;
        AddFriendFragment addFriendFragment;

        //Data
        List<User> users;
        FriendsListener friends;     
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);   
            SetContentView(Resource.Layout.activity_friends);
            users = new List<User>();
            friendsListAdapter = new FriendsListAdapter(users);
            GetFriends();
            ConnectViews();
            SetupRecyclerView();
            
        }
        private void ConnectViews()
        {   
            friendListRecyclerView = FindViewById<RecyclerView>(Resource.Id.friendListRecyclerView);
            backarrowFriendsImageView = FindViewById<ImageView>(Resource.Id.backarrowFriendsImageView);
            addFriendsImageView = FindViewById<ImageView>(Resource.Id.addFriendsImageView);
            addFriendsImageView.Click += AddFriendsImageView_Click;
            backarrowFriendsImageView.Click += (s, args) =>
              {
                  StartActivity(typeof(ProfileActivity));
                  Finish();
              };
        }

        private void AddFriendsImageView_Click(object sender, EventArgs e)
        {
            //Open Add Friend fragment
            ShowDialog();
        }

        private void SetupRecyclerView()
        {
                           
            friendListRecyclerView.SetLayoutManager(new LinearLayoutManager(friendListRecyclerView.Context));           
            friendListRecyclerView.SetAdapter(friendsListAdapter);
        }
        private void GetFriends()
        {
            friends = new FriendsListener(); 
            friends.FetchFriends();
            friends.listener.OnUserRetrieved += Listener_OnUserRetrieved;

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

        private void ShowDialog()
        {
            addFriendFragment = new AddFriendFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            addFriendFragment.Cancelable = true;
            addFriendFragment.Show(trans, "add_friend");
        }

        private void CloseDialog()
        {
            if (addFriendFragment != null)
            {
                addFriendFragment.Dismiss();
                addFriendFragment = null;
            }
        }
    }
}