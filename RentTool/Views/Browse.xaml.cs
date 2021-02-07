﻿using System;
using System.Collections.Generic;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentTool
{
    public partial class Browse : ContentPage
    {

        public string WebAPIkey = "AIzaSyBoxjCCLkKUoWxKJu4xAYLLyqf0krQwzoo";

        public Browse()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            GetProfileInformationAndRefreshToken();
        }


        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                MyUserName.Text = savedfirebaseauth.User.Email;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }



        }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            App.Current.MainPage = new NavigationPage(new MainPage());

        }
    }
}

