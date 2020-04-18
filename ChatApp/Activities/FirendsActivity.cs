﻿using System;
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
        Android.Support.V7.Widget.Toolbar toolbarFriends;
        RecyclerView friendListRecyclerView;
        FriendsListAdapter friendsListAdapter;
        ImageView backarrowFriendsImageView;
        ImageView addFriendsImageView;
        AddFriendFragment addFriendFragment;
        ManageFriendFragment manageFriendFragment;
        SendMessageManageFragment sendMessageManageFragment;

        //Data
        List<User> users;
        FriendsListener friends;
        FriendAddListener friendAddListener;
        SendMessageManageListener sendMessageManageListener;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);   
            SetContentView(Resource.Layout.activity_friends);
            users = new List<User>();
            friendsListAdapter = new FriendsListAdapter(users);
            friendsListAdapter.ItemClick += FriendsListAdapter_ItemClick;
            GetFriends();
            ConnectViews();
            SetupRecyclerView();
            
        }
        private void SetupToolbar()
        {
            toolbarFriends = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFriends);
            SetSupportActionBar(toolbarFriends);
        }

        private void FriendsListAdapter_ItemClick(object sender, FriendsListAdapterClickEventArgs e)
        {
            ShowManageFriendDialog(e.Position);
        }

        private void ConnectViews()
        {
            SetupToolbar();
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
            ShowAddFriendDialog();
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

        private void ShowAddFriendDialog()
        {
            friendAddListener = new FriendAddListener();           
            addFriendFragment = new AddFriendFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            addFriendFragment.Cancelable = true;          
            addFriendFragment.Show(trans, "add_friend");
            friendAddListener.OnResult += (s, args) =>
            {
                Toast.MakeText(this, args.Result, ToastLength.Long).Show();
                addFriendFragment.Dismiss();
            };
            addFriendFragment.OnFriendAdd += (s, args) =>
            {
                if (args.FriendEmail != Helpers.Helper.GetEmail())
                {
                    friendAddListener.InviteFriend(this, args.FriendEmail);
                }
                else
                {
                    Toast.MakeText(this, "You cannot invite yourself", ToastLength.Short).Show();
                }
                
            };
        }

        private void ShowManageFriendDialog(int position)
        {
            manageFriendFragment = new ManageFriendFragment(users[position]);
            var trans = SupportFragmentManager.BeginTransaction();
            manageFriendFragment.Cancelable = true;
            manageFriendFragment.OnNewMessageClicked += ManageFriendFragment_OnNewMessageClicked;
            manageFriendFragment.Show(trans, "manage_friend");
        }

        private void ManageFriendFragment_OnNewMessageClicked(object sender, ManageFriendFragment.NewMessageArgs e)
        {
            sendMessageManageFragment = new SendMessageManageFragment(e.UserArgs);
            var trans = SupportFragmentManager.BeginTransaction();
            sendMessageManageFragment.Cancelable = true;
            manageFriendFragment.Dismiss();           
            sendMessageManageFragment.OnMessageSent += SendMessageManageFragment_OnMessageSent;
            sendMessageManageFragment.Show(trans, "send_message");
        }

        private void SendMessageManageFragment_OnMessageSent(object sender, SendMessageManageFragment.MessageArgs e)
        {
             sendMessageManageListener = new SendMessageManageListener(FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid.ToString(),
               e.UserArgs.User_Id,
               e.BMessage.MessageBody,
               e.UserArgs.Fullname,
               e.UserArgs.Image_Url);
            sendMessageManageListener.OnSendingResult += (s, args) =>
              {
                  Toast.MakeText(this, args.Result, ToastLength.Long).Show();
              };
            sendMessageManageListener.SendMessage();
            sendMessageManageFragment.Dismiss();

        }
    }
}