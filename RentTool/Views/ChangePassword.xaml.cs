using System;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace RentTool.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePassword : ContentPage
    {

        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string token;
        

        public ChangePassword()
        {
           
            InitializeComponent();
            GetProfileInformationAndRefreshToken();
        }

        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth =
                    JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                        ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                token = savedfirebaseauth.FirebaseToken;
               
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }


        async void Change_Cliked(System.Object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth =
                    JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                        ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                token = savedfirebaseauth.FirebaseToken;
               
                var newPassword = UserNewPassword.Text;
                var verifyNewPassword = UserNPassword.Text;

                 // Check if both entries are same
                if (String.Equals(newPassword, verifyNewPassword))
                {
                    // change password 
                    await authProvider.ChangeUserPassword(token, newPassword);
                    await App.Current.MainPage.DisplayAlert("Alert", "Account Updated! ✅", "OK");
                    _= Navigation.PushAsync(new Account());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Please enter the same password on both Entry!", "OK");
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}