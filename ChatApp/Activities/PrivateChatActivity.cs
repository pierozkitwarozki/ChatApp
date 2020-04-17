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

namespace ChatApp.Activities
{
    [Activity(Label = "PrivateChatActivity")]
    public class PrivateChatActivity : AppCompatActivity
    {
        //Views
        TextView fullnamePrivateChateImageView;
        EditText typeMessagePrivateChatTextView;
        RecyclerView chatBodyPrivateChatRecyclerView;
        ImageView backarrowPrivateChatImageView;
        ImageView sendMessagePrivateChatImageView;

        //Adapters
        MessageListAdapter messageListAdapter;

        //MessagesListener
        MessagesListener listener;
        SendMessageManageListener send;

        //List
        List<BaseMessage> messagesList = new List<BaseMessage>();
        Conversation conversation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chat);
            // Create your application here
            conversation = JsonConvert.DeserializeObject<Conversation>(Intent.GetStringExtra("conv"));
            //Console.WriteLine(user_to.Fullname + user_to.Email);
            ConnectViews();
            GetMessages();
            SetupRecyclerView();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            listener.RemoveListener();
        }
        protected override void OnStop()
        {
            base.OnStop();
            listener.RemoveListener();
        }

        private void ConnectViews()
        {
            fullnamePrivateChateImageView = FindViewById<TextView>(Resource.Id.fullnamePrivateChateImageView);
            typeMessagePrivateChatTextView = FindViewById<EditText>(Resource.Id.typeMessagePrivateChatTextView);
            chatBodyPrivateChatRecyclerView = FindViewById<RecyclerView>(Resource.Id.chatBodyPrivateChatRecyclerView);
            backarrowPrivateChatImageView = FindViewById<ImageView>(Resource.Id.backarrowPrivateChatImageView);
            sendMessagePrivateChatImageView = FindViewById<ImageView>(Resource.Id.sendMessagePrivateChatImageView);
            sendMessagePrivateChatImageView.Click += SendMessagePrivateChatImageView_Click;
            fullnamePrivateChateImageView.Text = conversation.ProfileName;
            backarrowPrivateChatImageView.Click += BackarrowPrivateChatImageView_Click;
        }

        private void BackarrowPrivateChatImageView_Click(object sender, EventArgs e)
        {
            //listener.RemoveListener();
            StartActivity(typeof(MainActivity));
            Finish();
        }

        private void SendMessagePrivateChatImageView_Click(object sender, EventArgs e)
        {
            send = new SendMessageManageListener(Helpers.Helper.GetUserId(),
                conversation.UserId, typeMessagePrivateChatTextView.Text); ;
            send.SendMessage();
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
            listener.FetchMessages();
            listener.OnMessageRetrieved += (s, args) =>
              {
                  messagesList = args.MessageList;
                  if (messagesList != null)
                  {
                     messagesList.Sort((x, y) => DateTime.Compare(x.MessageDateTime, y.MessageDateTime));
                  }
                  SetupRecyclerView();
                  chatBodyPrivateChatRecyclerView.SmoothScrollToPosition(chatBodyPrivateChatRecyclerView.GetAdapter().ItemCount - 1);
              };

        }
    }
}