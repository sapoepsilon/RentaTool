using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentTool
{
    public partial class SignUp : ContentPage
    {

        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private string firstName;
        private string lastName;

        public SignUp()
        {
            InitializeComponent();
        }

        [Obsolete]
        async void signupbutton_Clicked(System.Object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(UserNewEmail.Text, UserNewPassword.Text);
                string gettoken = auth.FirebaseToken;

                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                // 1. Store the ID of the user
                var idUser = auth.User.LocalId;
                var toolId = new ArrayList();

                // 2. Create user in Firestore using the same id for the user created by Firebase
                await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("users")
                         .GetDocument(idUser)
                         .SetAsync(new { firstName = userFirstName.Text, lastName = userLastName.Text, zip = zip.Text,
                         phone = userPhone.Text});

                await App.Current.MainPage.DisplayAlert("Alert", "Account Created! ✅", "Ok");
                await Navigation.PushAsync(new MainContainerTabbedPage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "OK");
            }
        }
        
        

    }
}
