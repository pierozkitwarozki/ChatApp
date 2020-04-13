using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using ChatApp.DataModels;
using System.Collections.Generic;
using Refractored.Controls;
using ChatApp.EventListeners;

namespace ChatApp.Adapters
{
    class FriendsListAdapter : RecyclerView.Adapter
    {
        public event EventHandler<FriendsListAdapterClickEventArgs> ItemClick;
        public event EventHandler<FriendsListAdapterClickEventArgs> ItemLongClick;
        List<User> previewList;

        public FriendsListAdapter(List<User> _previewList)
        {
            previewList = new List<User>();
            previewList = _previewList;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.friend_preview, parent, false);

            var vh = new FriendsListAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = previewList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as FriendsListAdapterViewHolder;
            holder.namePreviewFriendsTextView.Text = previewList[position].Fullname;
            Helpers.Helper.GetCircleImage(previewList[position].Image_Url, holder.profilePreviewFriendsImageView);
        }

        public override int ItemCount => previewList.Count;

        void OnClick(FriendsListAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(FriendsListAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class FriendsListAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView namePreviewFriendsTextView { get; set; }
        public  _BaseCircleImageView profilePreviewFriendsImageView { get; set; }


        public FriendsListAdapterViewHolder(View itemView, Action<FriendsListAdapterClickEventArgs> clickListener,
                            Action<FriendsListAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            profilePreviewFriendsImageView = itemView.FindViewById<_BaseCircleImageView>(Resource.Id.profilePreviewFriendsImageView);
            namePreviewFriendsTextView = itemView.FindViewById<TextView>(Resource.Id.namePreviewFriendsTextView);
            itemView.Click += (sender, e) => clickListener(new FriendsListAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new FriendsListAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class FriendsListAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}