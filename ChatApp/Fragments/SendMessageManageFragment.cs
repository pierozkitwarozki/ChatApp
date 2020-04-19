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
using ChatApp.DataModels;
using ChatApp.EventListeners;

namespace ChatApp.Fragments
{
    public class SendMessageManageFragment : Android.Support.V4.App.DialogFragment
    {
        EditText messageBodyManageFriendEditText;
        Button sendManageFriendButton;
        User user;        

        public event EventHandler<MessageArgs> OnMessageSent;
        public class MessageArgs : EventArgs
        {
            public BaseMessage BMessage { get; set; }
            public User UserArgs { get; set; }
        }

        public SendMessageManageFragment(User _user)
        {
            user = _user;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.send_message, container, false);
            this.Dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            messageBodyManageFriendEditText = view.FindViewById<EditText>(Resource.Id.messageBodyManageFriendEditText);
            sendManageFriendButton = view.FindViewById<Button>(Resource.Id.sendManageFriendButton);
            sendManageFriendButton.Click += SendManageFriendButton_Click;
        }

        private void SendManageFriendButton_Click(object sender, EventArgs e)
        {
            OnMessageSent?.Invoke(this, new MessageArgs
            {
                BMessage = new BaseMessage { MessageBody = messageBodyManageFriendEditText.Text},
                UserArgs = user
            }); 
        }

    }
}