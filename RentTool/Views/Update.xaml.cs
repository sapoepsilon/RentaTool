using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;

namespace RentTool.Views
{
    
    public partial class Update : ContentPage
    {
        public string UserID;
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string token;
        public string user;

        public Update()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();


            // Function to vei
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
                UserID = savedfirebaseauth.User.LocalId;
                token = savedfirebaseauth.FirebaseToken;
                user = savedfirebaseauth.User.Email;
                QueryRequest();



            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }

        [Obsolete]
        async void Update_Cliked(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("users")
                    .GetDocument(UserID)
                    .UpdateAsync(new
                    {
                        firstName = userFirstName.Text,
                        lastName = userLastName.Text,
                        zip = zip.Text,
                        phone = userPhone.Text,
                        creditCardNumber = ccNumber.Text,
                        creditCardCvv = ccCvv.Text,
                        creditCardExpiration = ccMonthYear.Text
                    });
                await App.Current.MainPage.DisplayAlert("Alert", "Account Updated! ✅", "Ok");
                Navigation.PushAsync(new Account());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "OK");
            }
        }


        async void QueryRequest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                var document = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("users")
                    .GetDocument(UserID)
                    .GetAsync();

                var QueryObject = document.ToObject<Models.user>();

                userFirstName.Text = QueryObject.firstName;
                userLastName.Text = QueryObject.lastName;
                userPhone.Text = QueryObject.phone;
                ccCvv.Text = QueryObject.creditCardCvv;
                ccNumber.Text = QueryObject.creditCardNumber;
                ccMonthYear.Text = QueryObject.creditCardCvv;
                zip.Text = QueryObject.zip;
                UserNewEmail.Text = user;


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "can't Updated", "OK");
            }

        }

    }

}