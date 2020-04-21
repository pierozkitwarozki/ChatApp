using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using ChatApp.Adapters;
using System;
using ChatApp.EventListeners;
using ChatApp.DataModels;
using Android.Content;
using Newtonsoft.Json;
using ChatApp.Fragments;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbarMain;
        Android.Support.V4.Widget.SwipeRefreshLayout swipeRefresh;
        RecyclerView messagesRecyclerView;
        MessagePreviewAdapter messageAdapter;
        TextView toolbarMainTitle;
        ImageView messageImageView;
        ImageView groupChatImageView;
        ImageView createImageView;
        ImageView profileImageView;
        ImageView settingsImageView;

        //Fragments
        CreateFragment createFragment;
        CreateNewMessageFragment createNewMessageFragment;

        //Listners
        FullnameListener fullnameListener;
        ConversationListener conversationListener;


        //Data
        List<Conversation> conversations;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            conversations = new List<Conversation>();
            fullnameListener = new FullnameListener();  
            fullnameListener.FetchUser();
            ConnectViews();
            if (conversations.Count == 0)
            {   
                GetConversations();
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            GetConversations();
        }


        private void SetupToolbar()
        {
            toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbarMain);
        }

        private void ConnectViews()
        {
            SetupToolbar();
            swipeRefresh = FindViewById<Android.Support.V4.Widget.SwipeRefreshLayout>(Resource.Id.swipeRefresh);
            messagesRecyclerView = FindViewById<RecyclerView>(Resource.Id.postRecyclerView);
            toolbarMainTitle = FindViewById<TextView>(Resource.Id.titleMain);
            messageImageView = FindViewById<ImageView>(Resource.Id.messageImageView);
            groupChatImageView = FindViewById<ImageView>(Resource.Id.groupChatImageView);
            createImageView = FindViewById<ImageView>(Resource.Id.createImageView);
            profileImageView = FindViewById<ImageView>(Resource.Id.profileImageView);
            settingsImageView = FindViewById<ImageView>(Resource.Id.settingsImageView);
            groupChatImageView.Click += GroupChatImageView_Click;
            messageImageView.Click += MessageImageView_Click;
            profileImageView.Click += ProfileImageView_Click;
            settingsImageView.Click += SettingsImageView_Click;
            swipeRefresh.Refresh += SwipeRefresh_Refresh;
            createImageView.Click += CreateImageView_Click;
        }

        private void CreateImageView_Click(object sender, EventArgs e)
        {
            createImageView.Activated = !createImageView.Activated;
            ShowCreateNewDialog();
        }

        private void SwipeRefresh_Refresh(object sender, EventArgs e)
        {
            GetConversations();
            swipeRefresh.Refreshing = false;
        }

        private void SettingsImageView_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SettingsActivity));
        }

        private void ProfileImageView_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ProfileActivity));
        }

        private void MessageImageView_Click(object sender, EventArgs e)
        {
            //Open private chat
            toolbarMainTitle.Text = "Private chat";
            messageImageView.SetImageResource(Resource.Drawable.message_green);
            groupChatImageView.SetImageResource(Resource.Drawable.group_chat_grey);
        }

        private void GroupChatImageView_Click(object sender, EventArgs e)
        {
            //Open group chat
            //toolbarMainTitle.Text = "Group chat";
            //messageImageView.SetImageResource(Resource.Drawable.message_grey);
            //groupChatImageView.SetImageResource(Resource.Drawable.group_chat_green);
            Toast.MakeText(this, "Feature to be added in future...", ToastLength.Short).Show();
        }

        private void SetupRecyclerView()
        {
            messagesRecyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(messagesRecyclerView.Context));
            messageAdapter = new MessagePreviewAdapter(conversations);
            messagesRecyclerView.SetAdapter(messageAdapter);
            messageAdapter.ItemClick += MessageAdapter_ItemClick;
        }

        private void MessageAdapter_ItemClick(object sender, MessagePreviewAdapterClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(PrivateChatActivity));
            string conversation = JsonConvert.SerializeObject(conversations[e.Position]);
            intent.PutExtra("conv", conversation);
            StartActivity(intent);
        }

        private void GetConversations()
        {
            conversationListener = new ConversationListener();
            conversationListener.FetchConversations(this);
            conversationListener.OnConversationRetrieved += (s, args) =>
            {
                conversations = args.ConversationChat;
                if (conversations != null)
                {
                    conversations.Sort((x, y) => DateTime.Compare(y.LastMessageDate, x.LastMessageDate));                   
                }
                SetupRecyclerView();
            };
        }       

        private void ShowCreateNewDialog()
        {
            createFragment = new CreateFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            createFragment.OnNewGroupState += (s, e) => { Toast.MakeText(this, "New group", ToastLength.Short).Show(); };
            createFragment.OnNewMessageState += CreateFragment_OnNewMessageState;
            createFragment.Cancelable = true;
            createFragment.Show(trans, "create_fragment");

        }

        private void CreateFragment_OnNewMessageState(object sender, EventArgs e)
        {
            createFragment.Dismiss();
            createNewMessageFragment = new CreateNewMessageFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            createNewMessageFragment.Cancelable = true;
            createNewMessageFragment.Show(trans, "create_message");
        }
    }
}