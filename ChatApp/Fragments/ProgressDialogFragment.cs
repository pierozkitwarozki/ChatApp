using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ChatApp.Fragments
{
    public class ProgressDialogFragment : Android.Support.V4.App.DialogFragment
    {
        public string thisStatus;

        public ProgressDialogFragment(string status)
        {
            thisStatus = status;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.progress_dialog, container, false);
            TextView progressStatus = view.FindViewById<TextView>(Resource.Id.progressStatus);
            progressStatus.Text = thisStatus;
            this.Dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            return view;
        }
    }
}