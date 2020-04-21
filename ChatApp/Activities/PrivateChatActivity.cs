using System;
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
using Newtonsoft.Json;
using Refractored.Controls;

namespace ChatApp.Activities
{
    [Activity(Label = "PrivateChatActivity")]
    public class PrivateChatActivity : AppCompatActivity
    {
        //Views
        Android.Support.V7.Widget.Toolbar toolbarPrivateChat;
        TextView fullnamePrivateChateTextView;
        EditText typeMessagePrivateChatEditText;
        RecyclerView chatBodyPrivateChatRecyclerView;
        ImageButton backarrowPrivateChatImageView;
        ImageView sendMessagePrivateChatImageView;
        ImageView attachPhotoPrivateChatImageView;
        _BaseCircleImageView profileImagePrivateChatImageView;

        //Adapters
        MessageListAdapter messageListAdapter;

        //MessagesListener
        MessagesListener listener;
        SendMessageManageListener send;

        //Data
        List<BaseMessage> messagesList = new List<BaseMessage>();
        Conversation conversation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chat);
            conversation = JsonConvert.DeserializeObject<Conversation>(Intent.GetStringExtra("conv"));
            ConnectViews();
            GetMessages();
            SetupRecyclerView();
        }

        private void SetupToolbar()
        {
            toolbarPrivateChat = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarPrivateChat);
            SetSupportActionBar(toolbarPrivateChat);
        }

        private void ConnectViews()
        {
            SetupToolbar();
            profileImagePrivateChatImageView = FindViewById<_BaseCircleImageView>(Resource.Id.profileImagePrivateChatImageView);
            fullnamePrivateChateTextView = FindViewById<TextView>(Resource.Id.fullnamePrivateChateTextView);
            typeMessagePrivateChatEditText = FindViewById<EditText>(Resource.Id.typeMessagePrivateChatEditText);
            chatBodyPrivateChatRecyclerView = FindViewById<RecyclerView>(Resource.Id.chatBodyPrivateChatRecyclerView);
            backarrowPrivateChatImageView = FindViewById<ImageButton>(Resource.Id.backarrowPrivateChatImageView);
            sendMessagePrivateChatImageView = FindViewById<ImageView>(Resource.Id.sendMessagePrivateChatImageView);
            attachPhotoPrivateChatImageView = FindViewById<ImageView>(Resource.Id.attachPhotoPrivateChatImageView);
            sendMessagePrivateChatImageView.Click += SendMessagePrivateChatImageView_Click;
            fullnamePrivateChateTextView.Text = conversation.ProfileName;
            Helpers.Helper.GetCircleImage(conversation.ProfileImageUrl, profileImagePrivateChatImageView);
            backarrowPrivateChatImageView.Click += BackarrowPrivateChatImageView_Click;
            attachPhotoPrivateChatImageView.Click += (s, args) => { Toast.MakeText(this, "Feature to be added in future...", ToastLength.Short).Show(); };
        }

        private void BackarrowPrivateChatImageView_Click(object sender, EventArgs e)
        {
            //listener.RemoveListener();
            base.OnBackPressed();
        }

        private void SendMessagePrivateChatImageView_Click(object sender, EventArgs e)
        {
            if (typeMessagePrivateChatEditText.Text != "")
            {
                send = new SendMessageManageListener(Helpers.Helper.GetUserId(),
                conversation.UserId, typeMessagePrivateChatEditText.Text,
                conversation.ProfileName,
                conversation.ProfileImageUrl); ;
                typeMessagePrivateChatEditText.Text = "";
                chatBodyPrivateChatRecyclerView.ScrollToPosition(chatBodyPrivateChatRecyclerView.GetAdapter().ItemCount - 1);
                send.SendMessage();
            }
            
        }

        private void SetupRecyclerView()
        {
            messageListAdapter = new MessageListAdapter(messagesList);
            chatBodyPrivateChatRecyclerView.SetLayoutManager(new LinearLayoutManager(chatBodyPrivateChatRecyclerView.Context));
            chatBodyPrivateChatRecyclerView.SetAdapter(messageListAdapter);
            
        }

        private void GetMessages()
        {
            string id = conversation.ChatId;
            listener = new MessagesListener(id);
            listener.FetchMessages(this);
            listener.OnMessageRetrieved += (s, args) =>
              {
                  messagesList = args.MessageList;
                  if (messagesList != null)
                  {
                     messagesList.Sort((x, y) => DateTime.Compare(x.MessageDateTime, y.MessageDateTime));
                  }
                  SetupRecyclerView();
                  chatBodyPrivateChatRecyclerView.ScrollToPosition(chatBodyPrivateChatRecyclerView.GetAdapter().ItemCount - 1);
              };

        }
    }
}