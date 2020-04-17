using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using ChatApp.DataModels;
using Refractored.Controls;

namespace ChatApp.Adapters
{
    class MessageListAdapter : RecyclerView.Adapter
    {
        private const int VIEW_TYPE_MESSAGE_SENT = 1;
        private const int VIEW_TYPE_MESSAGE_RECEIVED = 2;

        //public event EventHandler<MessageListAdapterClickEventArgs> ItemClick;
        //public event EventHandler<MessageListAdapterClickEventArgs> ItemLongClick;
        private List<BaseMessage> messageList;

        public MessageListAdapter(List<BaseMessage> _messageList)
        {
            messageList = _messageList;
        }

        public override int GetItemViewType(int position)
        {
            BaseMessage message = messageList[position];
            if (message.ProfileFromId == FirebaseBackend.FirebaseBackend.GetFireAuth().CurrentUser.Uid.ToString())
            {
                return VIEW_TYPE_MESSAGE_SENT;
            }
            else
            {
                return VIEW_TYPE_MESSAGE_RECEIVED;
            }

        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            if(viewType == VIEW_TYPE_MESSAGE_SENT)
            {
                itemView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.item_message_sent, parent, false);
                return new SentMessageViewHolder(itemView);
            }
            else if(viewType == VIEW_TYPE_MESSAGE_RECEIVED)
            {
                itemView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.item_message_recieved, parent, false);
                return new RecievedMessageViewHolder(itemView);
            }

            //var vh = new MessageListAdapterViewHolder(itemView, OnClick, OnLongClick);
            return null;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = messageList[position];
            //var holder = viewHolder as MessageListAdapterViewHolder;
            //holder.TextView.Text = items[position];
            if (GetItemViewType(position) == 1)
            {
                ((SentMessageViewHolder)holder).Bind(item);
            }
            else if (GetItemViewType(position) == 2)
            {
                ((RecievedMessageViewHolder)holder).Bind(item);
            }

        }

        public override int ItemCount => messageList.Count;

        //void OnClick(MessageListAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        //void OnLongClick(MessageListAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

        private class RecievedMessageViewHolder : RecyclerView.ViewHolder
        {
            TextView profileMessageNameTextView, bodyMessageTextView, timeMessageTextView;
            _BaseCircleImageView profileMessageCircleImageView;
             public RecievedMessageViewHolder(View itemView) : base(itemView){

             profileMessageCircleImageView = itemView.FindViewById<_BaseCircleImageView>(Resource.Id.profileMessageCircleImageView);
             profileMessageNameTextView = itemView.FindViewById<TextView>(Resource.Id.profileMessageNameTextView);
             bodyMessageTextView = itemView.FindViewById<TextView>(Resource.Id.bodyMessageTextView);
             timeMessageTextView = itemView.FindViewById<TextView>(Resource.Id.timeMessageTextView);

             }

            public void Bind(BaseMessage message)
            {
                bodyMessageTextView.Text = message.MessageBody;
                timeMessageTextView.Text = message.MessageDateTime.Hour.ToString() + ":" + message.MessageDateTime.Minute.ToString();
                profileMessageNameTextView.Text = message.ProfileFromName;
                Helpers.Helper.GetCircleImage(message.ProfileImageId, profileMessageCircleImageView);
            }
 
        }

        private class SentMessageViewHolder : RecyclerView.ViewHolder
        {
            TextView bodyMessageTextView, timeMessageTextView;
            public SentMessageViewHolder(View itemView) : base(itemView)
            {
                bodyMessageTextView = itemView.FindViewById<TextView>(Resource.Id.bodyMessageTextView);
                timeMessageTextView = itemView.FindViewById<TextView>(Resource.Id.timeMessageTextView);
            }

            public void Bind(BaseMessage message)
            {
                bodyMessageTextView.Text = message.MessageBody;
                timeMessageTextView.Text = message.MessageDateTime.Hour.ToString() + ":" + message.MessageDateTime.Minute.ToString();
            }

        }

    }

}