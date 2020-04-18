using System;
using ChatApp.DataModels;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Refractored.Controls;

namespace ChatApp.Adapters
{
    class MessagePreviewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<MessagePreviewAdapterClickEventArgs> ItemClick;
        public event EventHandler<MessagePreviewAdapterClickEventArgs> ItemLongClick;
        List<Conversation> previewList;

        public MessagePreviewAdapter(List<Conversation> _previewList)
        {
            previewList = _previewList;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.message_preview, parent, false);

            var vh = new MessagePreviewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = previewList[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as MessagePreviewAdapterViewHolder;
            if (item.LastMessagePreview.Length > 45)
            {
                item.LastMessagePreview = item.LastMessagePreview.Substring(0, 45) + "...";
            }
            holder.textPreviewTextView.Text = item.LastMessagePreview;
            holder.namePreviewTextView.Text = item.ProfileName;
            Helpers.Helper.GetCircleImage(item.ProfileImageUrl, holder.profilePreviewImageView);
            holder.datePreviewTextView.Text = item.LastMessageDate.ToString("dd MM yyyy HH:mm:ss");
        }

        public override int ItemCount => previewList.Count;

        void OnClick(MessagePreviewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(MessagePreviewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class MessagePreviewAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public _BaseCircleImageView profilePreviewImageView;
        public TextView namePreviewTextView;
        public TextView textPreviewTextView;
        public TextView datePreviewTextView;

        public MessagePreviewAdapterViewHolder(View itemView, Action<MessagePreviewAdapterClickEventArgs> clickListener,
                            Action<MessagePreviewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            profilePreviewImageView = itemView.FindViewById<_BaseCircleImageView>(Resource.Id.profilePreviewImageView);
            namePreviewTextView = itemView.FindViewById<TextView>(Resource.Id.namePreviewTextView);
            textPreviewTextView = itemView.FindViewById<TextView>(Resource.Id.textPreviewTextView);
            datePreviewTextView = itemView.FindViewById<TextView>(Resource.Id.datePreviewTextView);
            itemView.Click += (sender, e) => clickListener(new MessagePreviewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new MessagePreviewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class MessagePreviewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}