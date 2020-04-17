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
using ChatApp.DataModels;
using Firebase.Firestore;

namespace ChatApp.EventListeners
{
    public class ConversationListener : Java.Lang.Object, Firebase.Firestore.IEventListener
    {
        public event EventHandler<ConversationArgs> OnConversationRetrieved;       
        public List<Conversation> Conversations = new List<Conversation>();
        

        public class ConversationArgs: EventArgs
        {
            public List<Conversation> ConversationChat { get; set; }
        }
        
        public void FetchConversations()
        {
            FirebaseBackend.FirebaseBackend
                .GetFireStore()
                .Collection("conversations")
                .AddSnapshotListener(this);
                
        }
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshotQuery = (QuerySnapshot)value;
            if (!snapshotQuery.IsEmpty)
            {
                if (Conversations.Count > 0)
                {
                    Conversations.Clear();
                }
                foreach (DocumentSnapshot snapshot in snapshotQuery.Documents)
                {
                    
                    if (snapshot.Id.Contains(Helpers.Helper.GetUserId()))
                    {
                        if (snapshot.Exists())
                        {
                            string chatId = snapshot.Id;
                            string lastMessage = "";
                            string userId = "";
                            string userImgUrl = "";
                            DateTime messageDate = DateTime.Parse("01/01/2000");
                            string userFullName = "";
                            foreach (KeyValuePair<string, Java.Lang.Object> pair in snapshot.Data)
                            {
                                string id = pair.Key.ToString(); //Field id
                                if(snapshot.Get(id + ".message_date") != null)
                                {
                                    string latestMessageTemp = snapshot.Get(id + ".message_body") != null ? snapshot.Get(id + ".message_body").ToString() : "";
                                    string userIdTemp = snapshot.Get(id + ".from") != null ? snapshot.Get(id + ".from").ToString() : "";
                                    string userImageUrlTemp = snapshot.Get(id + ".from_image_id") != null ? snapshot.Get(id + ".from_image_id").ToString() : "";
                                    string userNameTemp = snapshot.Get(id + ".from_fullname") != null ? snapshot.Get(id + ".from_fullname").ToString() : "";
                                    string messageDateTemp = snapshot.Get(id + ".message_date") != null ? snapshot.Get(id + ".message_date").ToString() : "";
                                    
                                    
                                    DateTime temp = DateTime.ParseExact(messageDateTemp, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    if (Helpers.Helper.GetUserId() != userIdTemp)
                                    {
                                        userId = userIdTemp;
                                        userImgUrl = userImageUrlTemp;
                                        userFullName = userNameTemp;
                                    }
                                    if(DateTime.Compare(messageDate, temp)<0)
                                    {
                                        messageDate = temp;
                                        lastMessage = latestMessageTemp;
                                    }

                                }
                                
                            }
                            Conversations.Add(new Conversation
                            {
                                ProfileImageUrl = userImgUrl,
                                LastMessageDate = messageDate,
                                UserId = userId,
                                ChatId = chatId,
                                LastMessagePreview = lastMessage,
                                ProfileName = userFullName
                            });
                            
                            
                        }

                    }
                }
                OnConversationRetrieved?.Invoke(this, new ConversationArgs
                {
                    ConversationChat = Conversations
                });
            }
            
        }

        public void RemoveListener()
        {
            var listener = FirebaseBackend.FirebaseBackend
               .GetFireStore()
               .Collection("conversations")
               .AddSnapshotListener(this);
            listener.Remove();
        }
    }

}