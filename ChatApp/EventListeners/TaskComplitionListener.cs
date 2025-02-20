﻿using System;
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

namespace ChatApp.EventListeners
{
    public class TaskComplitionListener: Java.Lang.Object, IOnSuccessListener, IOnFailureListener
    {
        //This class is responsible for handling logging in and registration results
        public event EventHandler<TaskSuccessEventArgs> Success;
        public event EventHandler<TaskFailureEventArgs> Failure;
        public class TaskFailureEventArgs : EventArgs
        {
            public string Cause { get; set; }
        }
        public class TaskSuccessEventArgs : EventArgs
        {
            public Java.Lang.Object Result { get; set; }
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            Failure?.Invoke(this, new TaskFailureEventArgs
            {
                Cause = e.Message
            });
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Success?.Invoke(this, new TaskSuccessEventArgs
            {
                Result = result
            });
        }
    }
}