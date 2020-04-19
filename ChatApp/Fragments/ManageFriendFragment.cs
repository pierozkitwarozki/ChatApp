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
using Refractored.Controls;

namespace ChatApp.Fragments
{
    public class ManageFriendFragment : Android.Support.V4.App.DialogFragment
    {
        _BaseCircleImageView profileImageManageImageView;
        TextView profileFullNameManageTextView;
        ImageView newMessageManageImageView;
        ImageView deleteFriendImageView;
        User user;

        public event EventHandler<NewMessageArgs> OnNewMessageClicked;
        public event EventHandler<NewMessageArgs> OnUserDeleteClicked;
        public class NewMessageArgs: EventArgs
        {
            public User UserArgs { get; set; }
        }

        public ManageFriendFragment(User _user)
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
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.manage_friend, container, false);
            this.Dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            ConnectViews(view);
            AssignData();
            return view;
        }

        private void ConnectViews(View view)
        {
            profileImageManageImageView = view.FindViewById<_BaseCircleImageView>(Resource.Id.profileImageManageImageView);
            profileFullNameManageTextView = view.FindViewById<TextView>(Resource.Id.profileFullNameManageTextView);
            newMessageManageImageView = view.FindViewById<ImageView>(Resource.Id.newMessageManageImageView);
            deleteFriendImageView = view.FindViewById<ImageView>(Resource.Id.deleteFriendImageView);
            newMessageManageImageView.Click += (s, args) =>
            {
                OnNewMessageClicked?.Invoke(this, new NewMessageArgs
                {
                    UserArgs = user
                });
            };
            deleteFriendImageView.Click += (s, args) =>
             {
                 OnUserDeleteClicked?.Invoke(this, new NewMessageArgs
                 {
                     UserArgs = user
                 });
             };
        }


        private void AssignData()
        {
            Helpers.Helper.GetCircleImage(user.Image_Url, profileImageManageImageView);
            profileFullNameManageTextView.Text = user.Fullname;
        }
    }
}