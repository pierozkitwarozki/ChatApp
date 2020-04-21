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
    public class CreateFragment : Android.Support.V4.App.DialogFragment
    {
        //Views
        ImageView newMessageCreateImageView;
        ImageView newGroupCreateImageView;

        public event EventHandler<EventArgs> OnNewMessageState;
        public event EventHandler<EventArgs> OnNewGroupState;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.create_new, container, false);
            this.Dialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            ConnectViews(view);
            return view;
        }

        private void ConnectViews(View view)
        {
            newMessageCreateImageView = view.FindViewById<ImageView>(Resource.Id.newMessageCreateImageView);
            newGroupCreateImageView = view.FindViewById<ImageView>(Resource.Id.newGroupCreateImageView);
            newMessageCreateImageView.Click += NewMessageCreateImageView_Click;
            newGroupCreateImageView.Click += NewGroupCreateImageView_Click;
        }

        private void NewGroupCreateImageView_Click(object sender, EventArgs e)
        {
            OnNewGroupState?.Invoke(this, e);
        }

        private void NewMessageCreateImageView_Click(object sender, EventArgs e)
        {
            OnNewMessageState?.Invoke(this, e);
        }
    }
}