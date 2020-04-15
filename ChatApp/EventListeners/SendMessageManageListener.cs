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
using Firebase.Firestore;
using Java.Util;

namespace ChatApp.EventListeners
{
    public class SendMessageManageListener: Java.Lang.Object, IOnSuccessListener
    {
        string conversationId;
        string from;
        string to;
        string body;

        public SendMessageManageListener(string _from, string _to, string _text)
        {
            body = _text;
            from = _from;
            to = _to;
            conversationId = GenerateChatId(from, to);
        }


        public void SendMessage()
        {
            FirebaseBackend.FirebaseBackend.GetFireStore()
                 .Collection("conversations")
                 .Document(conversationId).Get().AddOnSuccessListener(this);    
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            string messageId = FirebaseBackend.FirebaseBackend.GetFireStore()
                   .Collection("do not exits")
                   .Document(GenerateMessageId()).Id.ToString();
            DocumentReference documentRef = FirebaseBackend.FirebaseBackend.GetFireStore()
             .Collection("conversations")
             .Document(conversationId);
            if (!snapshot.Exists())
            {
                    HashMap hashMap = new HashMap();
                    hashMap.Put(messageId, null);
                    documentRef.Set(hashMap);
                    documentRef.Update(messageId.ToString() + "." + "message_body", body);
                    documentRef.Update(messageId.ToString() + "." + "date", DateTime.Now.ToString());
                    documentRef.Update(messageId.ToString() + "." + "from", from);
                    documentRef.Update(messageId.ToString() + "." + "to", to);
            }
            else
            {
                HashMap hashMap = new HashMap();
                hashMap.Put(messageId, null);
                documentRef.Update(messageId, null);
                documentRef.Update(messageId.ToString() + "." + "message_body", body);
                documentRef.Update(messageId.ToString() + "." + "date", DateTime.Now.ToString());
                documentRef.Update(messageId.ToString() + "." + "from", from);
                documentRef.Update(messageId.ToString() + "." + "to", to);
            }
            
        }

        private string GenerateChatId(string id1, string id2)
        {
                List<int> wordOne = new List<int>();
                List<int> wordTwo = new List<int>();
                foreach (char c in id1)
                {
                    wordOne.Add(Convert.ToInt32(c));
                }
                foreach (char c in id2)
                {
                    wordTwo.Add(Convert.ToInt32(c));
                }
                int sumOne = 0;
                int sumTwo = 0;
                foreach (int i in wordOne)
                {
                    sumOne = sumOne + i;
                }
                foreach (int i in wordTwo)
                {
                    sumTwo = sumTwo + i;
                }

                double averageOne = (double)sumOne / (double)wordOne.Max();
                double averageTwo = (double)sumTwo / (double)wordTwo.Max();

                if (averageOne < averageTwo)
                {
                    return id1 + "_" + id2;
                }
                else return id2 + "_" + id1;
        }

        private string GenerateMessageId()
        {
            System.Random rand = new System.Random();
            int length = rand.Next(0, 15);
            char[] allowchars = "ABCDEFGHIJKLOMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string sResult = "";
            for (int i = 0; i <= length; i++)
            {
                sResult += allowchars[rand.Next(0, allowchars.Length)];
            }

            return sResult;

        }

    }
}