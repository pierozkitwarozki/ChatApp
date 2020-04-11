using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

namespace ChatApp.FirebaseBackend
{
    public static class FirebaseBackend
    {
        public static FirebaseFirestore GetFireStore()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseFirestore database;
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("chatappxam")
                    .SetApplicationId("chatappxam")
                    .SetApiKey("AIzaSyCl4DPQCIgHPRs3KrkrmeCeYCDL7lEdho4")
                    .SetDatabaseUrl("https://chatappxam.firebaseio.com")
                    .SetStorageBucket("chatappxam.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }
        public static FirebaseAuth GetFireAuth()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseAuth auth;
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("chatappxam")
                    .SetApplicationId("chatappxam")
                    .SetApiKey("AIzaSyCl4DPQCIgHPRs3KrkrmeCeYCDL7lEdho4")
                    .SetDatabaseUrl("https://chatappxam.firebaseio.com")
                    .SetStorageBucket("chatappxam.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
                auth = FirebaseAuth.Instance;
            }
            else
            {
                auth = FirebaseAuth.Instance;
            }
            return auth;
        }
    }
}