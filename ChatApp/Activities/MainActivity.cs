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

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbarMain;       
        RecyclerView messagesRecyclerView;
        MessagePreviewAdapter messageAdapter;
        TextView toolbarMainTitle;
        ImageView messageImageView;
        ImageView groupChatImageView;
        ImageView createImageView;
        ImageView profileImageView;
        ImageView settingsImageView;

        //Listners
        FullnameListener fullnameListener;
        

        //Data
        List<ChatApp.DataModels.Message> messages;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            messages = new List<ChatApp.DataModels.Message>();
            fullnameListener = new FullnameListener();
            
            fullnameListener.FetchUser();
            CreateData();
            ConnectViews();
            SetupRecyclerView();

        }
        private void SetupToolbar()
        {
            toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbarMain);
        }

        private void ConnectViews()
        {
            SetupToolbar();
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
            toolbarMainTitle.Text = "Group chat";
            messageImageView.SetImageResource(Resource.Drawable.message_grey);
            groupChatImageView.SetImageResource(Resource.Drawable.group_chat_green);
        }

        private void SetupRecyclerView()
        {
            messagesRecyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(messagesRecyclerView.Context));
            messageAdapter = new MessagePreviewAdapter(messages);
            messagesRecyclerView.SetAdapter(messageAdapter);
        }

        private void CreateData()
        {
            messages.Add(
                new ChatApp.DataModels.Message
                {
                    MessageDateTime = DateTime.Now,
                    ProfileName = "Michael Jordan",
                    MessageShort = "What's up??"
                }
            );
            messages.Add(
                new ChatApp.DataModels.Message
                {
                    MessageDateTime = DateTime.Now,
                    ProfileName = "Michael Jordan",
                    MessageShort = "What's up??"
                }
            );
            messages.Add(
                new ChatApp.DataModels.Message
                {
                    MessageDateTime = DateTime.Now,
                    ProfileName = "Michael Jordan",
                    MessageShort = "What's up??"
                }
            );
            messages.Add(
                new ChatApp.DataModels.Message
                {
                    MessageDateTime = DateTime.Now,
                    ProfileName = "Michael Jordan",
                    MessageShort = "What's up??"
                }
            ); messages.Add(
                 new ChatApp.DataModels.Message
                 {
                     MessageDateTime = DateTime.Now,
                     ProfileName = "Michael Jordan",
                     MessageShort = "What's up??"
                 }
             ); messages.Add(
                 new ChatApp.DataModels.Message
                 {
                     MessageDateTime = DateTime.Now,
                     ProfileName = "Michael Jordan",
                     MessageShort = "What's up??"
                 }
             ); messages.Add(
                 new ChatApp.DataModels.Message
                 {
                     MessageDateTime = DateTime.Now,
                     ProfileName = "Michael Jordan",
                     MessageShort = "What's up??"
                 }
             ); messages.Add(
                 new ChatApp.DataModels.Message
                 {
                     MessageDateTime = DateTime.Now,
                     ProfileName = "Michael Jordan",
                     MessageShort = "What's up??"
                 }
             ); messages.Add(
                 new ChatApp.DataModels.Message
                 {
                     MessageDateTime = DateTime.Now,
                     ProfileName = "Michael Jordan",
                     MessageShort = "What's up??"
                 }
             );
        }
        


    }
}