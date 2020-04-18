using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChatApp.Activities;
using Firebase.Firestore;
using Java.Util;

namespace ChatApp.EventListeners
{
    public class SendMessageManageListener: Java.Lang.Object, IOnSuccessListener, IOnFailureListener
    {
        string conversationId;
        string from;
        string to;
        string body;
        string toFullname;
        string toUrl;

        public event EventHandler<ResultEventArgs> OnSendingResult;
        public class ResultEventArgs: EventArgs
        {
            public string Result { get; set; }
        }

        public SendMessageManageListener(string _from, string _to, string _text, string _toFullname, string _toUrl)
        {
            body = _text;
            from = _from;
            to = _to;
            toFullname = _toFullname;
            toUrl = _toUrl;
            conversationId = Helpers.Helper.GenerateChatId(from, to);
        }


        public void SendMessage()
        {
            FirebaseBackend.FirebaseBackend.GetFireStore()
                 .Collection("chats")
                 .Document(conversationId)
                 .Get()
                 .AddOnSuccessListener(this)
                 .AddOnFailureListener(this);    
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            
            var snapshot = (DocumentSnapshot)result;
            string messageId = FirebaseBackend.FirebaseBackend.GetFireStore()
                   .Collection("do not exits")
                   .Document(Helpers.Helper.GenerateMessageId()).Id.ToString();
            DocumentReference documentRef = FirebaseBackend.FirebaseBackend.GetFireStore()
             .Collection("chats")
             .Document(conversationId);
            if (!snapshot.Exists())
            {
                    HashMap hashMap = new HashMap();
                    hashMap.Put(messageId, null);
                    documentRef.Set(hashMap);
                    documentRef.Update(messageId.ToString() + "." + "message_body", body);
                    documentRef.Update(messageId.ToString() + "." + "message_date", DateTime.Now.ToString("dd MM yyyy HH:mm:ss"));
                    documentRef.Update(messageId.ToString() + "." + "from", from);
                    documentRef.Update(messageId.ToString() + "." + "to", to);
                    documentRef.Update(messageId.ToString() + "." + "to_fullname", toFullname);
                    documentRef.Update(messageId.ToString() + "." + "to_image_id", toUrl);
                    documentRef.Update(messageId.ToString() + "." + "from_fullname", Helpers.Helper.GetFullName());
                    documentRef.Update(messageId.ToString() + "." + "from_image_id", Helpers.Helper.GetImageUrl());
                    
            }
            else
            {
                HashMap hashMap = new HashMap();
                hashMap.Put(messageId, null);
                documentRef.Update(messageId, null);
                documentRef.Update(messageId.ToString() + "." + "message_body", body);
                documentRef.Update(messageId.ToString() + "." + "message_date", DateTime.Now.ToString("dd MM yyyy HH:mm:ss"));
                documentRef.Update(messageId.ToString() + "." + "from", from);
                documentRef.Update(messageId.ToString() + "." + "to", to);
                documentRef.Update(messageId.ToString() + "." + "to_fullname", toFullname);
                documentRef.Update(messageId.ToString() + "." + "to_image_id", toUrl);
                documentRef.Update(messageId.ToString() + "." + "from_fullname", Helpers.Helper.GetFullName());
                documentRef.Update(messageId.ToString() + "." + "from_image_id", Helpers.Helper.GetImageUrl());
            }
            OnSendingResult?.Invoke(this, new ResultEventArgs
            {
                Result = "Message sent successfuly"
            });
            
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            OnSendingResult?.Invoke(this, new ResultEventArgs
            {
                Result = "Error occured: " + e.Message
            }); 
        }
    }
}