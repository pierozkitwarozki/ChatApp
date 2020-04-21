using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ChatApp.Adapters;
using ChatApp.DataModels;
using ChatApp.EventListeners;

namespace ChatApp.Fragments
{
    public class CreateNewMessageFragment : Android.Support.V4.App.DialogFragment
    {
        //Views
        Spinner friendsSpinner;
        EditText messageBodyCreateMessageEditText;
        Button sendCreateMessageButton;

        //Data
        List<User> users;
        User thisUser;

        //Listeners
        FriendsListener friends;
        SendMessageManageListener sendMessageManageListener;

        //Adapters
        FriendArrayAdapter friendArrayAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.create_new_message, container, false);
            users = new List<User>();
            
            this.Dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            GetFriends();
            ConnectViews(view);           
            SetupSpinner();
            return view;
        }

        private void ConnectViews(View view)
        {
            friendsSpinner = view.FindViewById<Spinner>(Resource.Id.friendsSpinner);
            messageBodyCreateMessageEditText = view.FindViewById<EditText>(Resource.Id.messageBodyCreateMessageEditText);
            sendCreateMessageButton = view.FindViewById<Button>(Resource.Id.sendCreateMessageButton);
            friendsSpinner.ItemSelected += FriendsSpinner_ItemSelected;
            sendCreateMessageButton.Click += SendCreateMessageButton_Click;
            
        }

        private void SendCreateMessageButton_Click(object sender, EventArgs e)
        {
            if (messageBodyCreateMessageEditText.Text != "")
            {
                sendMessageManageListener = new SendMessageManageListener(FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid.ToString(),
                thisUser.User_Id,
                messageBodyCreateMessageEditText.Text,
                thisUser.Fullname,
                thisUser.Image_Url);
                sendMessageManageListener.OnSendingResult += (s, args) =>
                {
                    Toast.MakeText(Application.Context, args.Result, ToastLength.Long).Show();
                };
                sendMessageManageListener.SendMessage();
                this.Dismiss();
            }
            else Toast.MakeText(this.Context, "Please enter a message", ToastLength.Long).Show();

        }


        private void FriendsSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            thisUser = users[e.Position];
        }

        private void GetFriends()
        {
            friends = new FriendsListener();
            friends.FetchFriends();
            friends.listener.OnUserRetrieved += Listener_OnUserRetrieved;
        }

        private void SetupSpinner()
        {
            friendArrayAdapter = new FriendArrayAdapter(Context, users);            
            friendsSpinner.Adapter = friendArrayAdapter;
        }

        private void Listener_OnUserRetrieved(object sender, UserDataListener.UserEventArgs e)
        {
            users.Add(e.UserData);
            if (users.Count > 0)
            {
                users.Sort((x, y) => String.Compare(x.Fullname, y.Fullname));
            }
            SetupSpinner();
        }


    }
}