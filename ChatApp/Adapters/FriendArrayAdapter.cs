using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChatApp.DataModels;
using Refractored.Controls;

namespace ChatApp.Adapters
{
    class FriendArrayAdapter : ArrayAdapter<User>
    {
        List<User> users = new List<User>();
        public FriendArrayAdapter(Context context, List<User> _users) : base(context, 0, _users)
        {
            users = _users;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return InitView(position, convertView, parent);
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            return InitView(position, convertView, parent);
        }

        private View InitView(int position, View convertView, ViewGroup parent)
        {
            if(convertView == null)
            {
                convertView = LayoutInflater.From(Context).Inflate(
                    Resource.Layout.friend_preview, parent, false);
            }
            _BaseCircleImageView profilePreviewFriendsImageView = convertView.FindViewById<_BaseCircleImageView>(Resource.Id.profilePreviewFriendsImageView);
            TextView namePreviewFriendsTextView = convertView.FindViewById<TextView>(Resource.Id.namePreviewFriendsTextView);
            profilePreviewFriendsImageView.ScaleX = (float)0.6;
            profilePreviewFriendsImageView.ScaleY = (float)0.6;
            namePreviewFriendsTextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)13);
            User item = users[position];
            if (item != null)
            {
                namePreviewFriendsTextView.Text = item.Fullname;
                Helpers.Helper.GetCircleImage(item.Image_Url, profilePreviewFriendsImageView);
            }
            return convertView;
        }

    }
}