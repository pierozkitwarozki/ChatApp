using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ChatApp.EventListeners;
using ChatApp.Fragments;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Storage;
using Java.Util;
using Plugin.Media;
using Refractored.Controls;

namespace ChatApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SignUpActivity : AppCompatActivity
    {
        //Controls
        Android.Support.V7.Widget.Toolbar toolbar;
        EditText fullnameSignUpEditText;
        EditText emailSignUpEditText;
        EditText passwordSignUpEditText;
        EditText confirmPasswordSignUpEditText;
        Button signUpButton;
        ImageButton backarrowImageView;
        _BaseCircleImageView registerCircleImageView;
        ProgressDialogFragment progressDialogFragment;

        //Firebase
        FirebaseFirestore database;
        FirebaseAuth auth;
        TaskComplitionListener listener;
        TaskComplitionListener downloadUrlListener;
        TaskComplitionListener registrationListener;

        //Permissions
        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        //ByteArray for image
        byte[] fileBytes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup);
            auth = FirebaseBackend.FirebaseBackend.GetFireAuth();
            database = FirebaseBackend.FirebaseBackend.GetFireStore();
            listener = new TaskComplitionListener();
            downloadUrlListener = new TaskComplitionListener();
            registrationListener = new TaskComplitionListener();
            SetupToolbar();
            ConnectViews();
            RequestPermissions(permissionGroup, 0);
            // Create your application here
        }

        private void ConnectViews()
        {
            fullnameSignUpEditText = FindViewById<EditText>(Resource.Id.fullnameSignUpEditText);
            emailSignUpEditText = FindViewById<EditText>(Resource.Id.emailSignUpEditText);
            passwordSignUpEditText = FindViewById<EditText>(Resource.Id.passwordSignUpEditText);
            confirmPasswordSignUpEditText = FindViewById<EditText>(Resource.Id.confirmPasswordSignUpEditText);
            signUpButton = FindViewById<Button>(Resource.Id.signUpButton);
            registerCircleImageView = FindViewById<_BaseCircleImageView>(Resource.Id.registerCircleImageView);
            backarrowImageView = FindViewById<ImageButton>(Resource.Id.backarrowImageView);
            signUpButton.Click += SignUpButton_Click;
            backarrowImageView.Click += BackarrowImageView_Click;
            registerCircleImageView.Click += RegisterCircleImageView_Click;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void RegisterCircleImageView_Click(object sender, EventArgs e)
        {
            Android.Support.V7.App.AlertDialog.Builder photoAlert =
                new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            photoAlert.SetMessage("Add photo");

            photoAlert.SetNegativeButton("Take photo", (thisalert, args) =>
            {
                //Capture photo
                TakePhotoAsync();
            });
            photoAlert.SetPositiveButton("Choose from gallery", (thisalert, args) =>
            {
                //Choose from gallery
                SelectPhoto();
            });
            photoAlert.Show();
        }

        private void BackarrowImageView_Click(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }

        private void SetupToolbar()
        {
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            SignUp(fullnameSignUpEditText.Text,
                emailSignUpEditText.Text,               
                passwordSignUpEditText.Text,
                confirmPasswordSignUpEditText.Text);

        }

        private async void TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 20,
                Directory = "Sample",
                Name = Helpers.Helper.GenerateRandomString(6) + "facepost.jpg"
            });
            if (file == null)
            {
                return;
            }
            //converts file.path to byte array and set the resulting bitmap to imageview
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            fileBytes = imageArray;

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            registerCircleImageView.SetImageBitmap(bitmap);
        }
        private async void SelectPhoto()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Upload photos not supported", ToastLength.Short).Show();
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 20
            });
            if (file == null)
            {
                return;
            }
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            fileBytes = imageArray;

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            registerCircleImageView.SetImageBitmap(bitmap);
        }
        private void SignUp(string fullname, string email, string password, string confirmPassword)
        {
            //Register funcionality
            if (!Helpers.Helper.IsValidEmail(email))
            {
                Toast.MakeText(this, "Please provide a valid email", ToastLength.Short).Show();
                return;
            }
            else if (password != confirmPassword)
            {
                Toast.MakeText(this, "Passwords does not match", ToastLength.Short).Show();
                return;
            }
            ShowDialog("Registering...");
            auth.CreateUserWithEmailAndPassword(email, password)
                .AddOnFailureListener(registrationListener)
                .AddOnSuccessListener(registrationListener);

            string downloadUrl = "";
            HashMap userMap = new HashMap();
            StorageReference storageReference = null;
            DocumentReference userReference = null;
          
            //Registration Success
            registrationListener.Success += (s, args) =>
            {

                userReference = database
                          .Collection("users")
                          .Document(auth.CurrentUser.Uid);
                
                string userId = userReference.Id;
                userMap.Put("email", email);
                userMap.Put("fullname", fullname);
                userReference.Set(userMap);
                SetupPrototipeCollections();
                if (fileBytes != null)
                {
                    Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                    storageReference = FirebaseStorage.Instance.GetReference("chatAppAvatars/" + userId);
                    storageReference.PutBytes(fileBytes)
                        .AddOnSuccessListener(listener)
                        .AddOnFailureListener(listener);
                }
                else
                {
                    CloseDialog();
                    StartActivity(typeof(LoginActivity));
                    Finish();
                    Toast.MakeText(this, "Registration succeed", ToastLength.Short).Show();
                    return;
                }
            };

            //Registration Failure callback
            registrationListener.Failure += (f, args) =>
            {
                CloseDialog();
                Toast.MakeText(this, "Registration failed due to: " + args.Cause, ToastLength.Short).Show();
            };

            //Image Add  success
            listener.Success += (success, args) =>
            {
                if (storageReference != null)
                {
                    Console.WriteLine("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
                    storageReference.DownloadUrl.AddOnSuccessListener(downloadUrlListener);
                    //storageReference.DownloadUrl.AddOnSuccessListener(downloadUrlListener);
                }
                Console.WriteLine("NUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUL");
            };
            downloadUrlListener.Success += (success, args) =>
            {
                downloadUrl = args.Result.ToString();
                userReference = database
                          .Collection("users")
                          .Document(auth.CurrentUser.Uid);
                userMap.Put("image_id", downloadUrl);
                userReference.Set(userMap);
                CloseDialog();
                Toast.MakeText(this, "Registration succeed", ToastLength.Short).Show();
                StartActivity(typeof(LoginActivity));
                Finish();
            };
            listener.Failure += (f, args) =>
            {
                Toast.MakeText(this, "Cannot upload the image", ToastLength.Short).Show();
                CloseDialog();
            };
        }

        private void ShowDialog(string status)
        {
            progressDialogFragment = new ProgressDialogFragment(status);
            var trans = SupportFragmentManager.BeginTransaction();
            progressDialogFragment.Cancelable = false;
            progressDialogFragment.Show(trans, "progress");

        }

        private void CloseDialog()
        {
            if (progressDialogFragment != null)
            {
                progressDialogFragment.Dismiss();
                progressDialogFragment = null;
            }
        }

        private void SetupPrototipeCollections()
        {
            //To będzie do zmiany
            DocumentReference reff = FirebaseBackend.FirebaseBackend.GetFireStore()
                .Collection("invitations").Document(auth.CurrentUser.Uid);
            HashMap hash = new HashMap();
            hash.Put("none", true);
            reff.Set(hash);

            DocumentReference refff = FirebaseBackend.FirebaseBackend.GetFireStore()
                .Collection("friends").Document(auth.CurrentUser.Uid);
            HashMap hashh = new HashMap();
            hashh.Put("none", true);
            refff.Set(hashh);

        }
    }
}