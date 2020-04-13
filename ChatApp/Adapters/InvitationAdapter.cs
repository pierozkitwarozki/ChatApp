using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ChatApp.DataModels;
using Refractored.Controls;
using ChatApp.EventListeners;

namespace ChatApp.Adapters
{
    public class InvitationAdapter : RecyclerView.Adapter
    {
        public event EventHandler<InvitationAdapterClickEventArgs> ItemClick;
        public event EventHandler<InvitationAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<InvitationAdapterClickEventArgs> AcceptClick;
        public event EventHandler<InvitationAdapterClickEventArgs> DeleteClick;
        List<User> previewList;

        public InvitationAdapter(List<User> _previewList)
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
                   Inflate(Resource.Layout.invitation, parent, false);

            var vh = new InvitationAdapterViewHolder(itemView, OnClick, OnLongClick, OnAcceptClick, OnDeleteClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = previewList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as InvitationAdapterViewHolder;
            holder.namePreviewInviteXTextView.Text = previewList[position].Fullname;
            Helpers.Helper.GetCircleImage(previewList[position].Image_Url, holder.profilePreviewInviteXImageView);
        }

        public override int ItemCount => previewList.Count;

        void OnClick(InvitationAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(InvitationAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

        void OnAcceptClick(InvitationAdapterClickEventArgs args) => AcceptClick?.Invoke(this, args);
        void OnDeleteClick(InvitationAdapterClickEventArgs args) => DeleteClick?.Invoke(this, args);

    }

    public class InvitationAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView namePreviewInviteXTextView { get; set; }
        public _BaseCircleImageView profilePreviewInviteXImageView { get; set; }
        public ImageView acceptFriendXImageView { get; set; }
        public ImageView deletetFriendXImageView { get; set; }

        public InvitationAdapterViewHolder(View itemView, Action<InvitationAdapterClickEventArgs> clickListener,
                            Action<InvitationAdapterClickEventArgs> longClickListener,
                            Action<InvitationAdapterClickEventArgs> acceptClickListener,
                            Action<InvitationAdapterClickEventArgs> deleteClickListener) : base(itemView)
        {
            namePreviewInviteXTextView = itemView.FindViewById<TextView>(Resource.Id.namePreviewInviteXTextView);
            profilePreviewInviteXImageView = itemView.FindViewById<_BaseCircleImageView>(Resource.Id.profilePreviewInviteXImageView);
            acceptFriendXImageView = itemView.FindViewById<ImageView>(Resource.Id.acceptFriendXImageView);
            deletetFriendXImageView = itemView.FindViewById<ImageView>(Resource.Id.deletetFriendXImageView);

            itemView.Click += (sender, e) => clickListener(new InvitationAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new InvitationAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            acceptFriendXImageView.Click += (sender, e) => acceptClickListener(new InvitationAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            deletetFriendXImageView.Click += (sender, e) => deleteClickListener(new InvitationAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class InvitationAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}