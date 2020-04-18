using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChatApp.Activities;
using ChatApp.DataModels;
using Firebase.Firestore;

namespace ChatApp.EventListeners
{
    public class MessagesListener : Java.Lang.Object, Firebase.Firestore.IEventListener
    {
        //This class implements real-time fetching messages in a single conversation
        public List<BaseMessage> messageList = new List<BaseMessage>();
        private string chatId;

        public MessagesListener(string _chatID)
        {
            chatId = _chatID;
        }
        public void FetchMessages(PrivateChatActivity activity)
        {
            FirebaseBackend.FirebaseBackend.GetFireStore()
                .Collection("chats")
                .Document(chatId)
                .AddSnapshotListener(activity, this);
        }

        public event EventHandler<MessagesEventArgs> OnMessageRetrieved;
        public class MessagesEventArgs : EventArgs
        {
            public List<BaseMessage> MessageList { get; set; }
        }

        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
                var querySnapshot = (DocumentSnapshot)value;
                if (querySnapshot.Exists())
                {
                    if (messageList.Count > 0)
                    {
                        messageList.Clear();
                    }
                    foreach (KeyValuePair<string, Java.Lang.Object> pair in querySnapshot.Data)
                    {
                    
                    string id = pair.Key.ToString(); //Field id
                        if (querySnapshot.Get(id + ".message_date") != null)
                        {
                        string image_url = querySnapshot.Get(id + ".from_image_id") != null ? querySnapshot.Get(id + ".from_image_id").ToString() : "";
                        string messageDate = querySnapshot.Get(id + ".message_date") != null ? querySnapshot.Get(id + ".message_date").ToString() : "";

                        BaseMessage message = new BaseMessage
                        {
                            ProfileImageId = image_url,
                            ProfileFromId = querySnapshot.Get(id + ".from") != null ? querySnapshot.Get(id + ".from").ToString() : "",
                            ProfileFromName = querySnapshot.Get(id + ".from_fullname") != null ? querySnapshot.Get(id + ".from_fullname").ToString() : "",
                            ProfileToId = querySnapshot.Get(id + ".to") != null ? querySnapshot.Get(id + ".to").ToString() : "",
                            MessageBody = querySnapshot.Get(id + ".message_body") != null ? querySnapshot.Get(id + ".message_body").ToString() : "",
                            //MessageDateTime = DateTime.ParseExact(messageDate, format, null)
                        };
                        message.MessageDateTime = DateTime.ParseExact(messageDate, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture); 
                        messageList.Add(message);
                        }
                        
                    }

                    OnMessageRetrieved?.Invoke(this, new MessagesEventArgs
                    {
                        MessageList = messageList
                    });
                }
        }
    }
}