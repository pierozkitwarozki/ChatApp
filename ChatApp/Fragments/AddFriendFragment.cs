using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.EventListeners;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ChatApp.Fragments
{
    public class AddFriendFragment : Android.Support.V4.App.DialogFragment
    {
        //Controls
        EditText inviteEditText;
        Button inviteButton;

        public event EventHandler<AddFriendEventArgs> OnFriendAdd;
        public class AddFriendEventArgs: EventArgs
        {
            public string FriendEmail { get; set; }
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.add_friend, container, false);
            this.Dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            inviteButton = view.FindViewById<Button>(Resource.Id.inviteButton);
            inviteEditText = view.FindViewById<EditText>(Resource.Id.inviteEditText);
            inviteButton.Click += InviteButton_Click;
            return view;
        }

        private void InviteButton_Click(object sender, EventArgs e)
        {

            OnFriendAdd?.Invoke(this, new AddFriendEventArgs
            {
                FriendEmail = inviteEditText.Text
            });

        }
    }
}