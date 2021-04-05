using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using RentTool.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentTool
{
    public partial class SignIn : ContentPage
    {

        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";

        public SignIn()
        {
            InitializeComponent();
            RotateElement(logoImage, CancellationToken.None);
        }

        async void loginbutton_Clicked(System.Object sender, System.EventArgs e)
        {

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserLoginEmail.Text, UserLoginPassword.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);
                await Navigation.PushAsync(new MainContainerTabbedPage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        void Reset_Clicked(System.Object Sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ResetPassword());

        }

        void NavToSignUp_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SignUp());

        }
        private async Task RotateElement(VisualElement element, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                await logoImage.RotateTo(360, 24000, Easing.Linear);
                await logoImage.RotateTo(0, 0); // reset to initial position
            }
        }
    } 

}
