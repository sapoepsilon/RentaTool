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

namespace RentTool
{
    public partial class Account : ContentPage
    {

        public string UserID;
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";

        public Account()
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
                QueryRequest();


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }



        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddNewTool());
        }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

        async void UpdateClicked(object sender, EventArgs e)
        {
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
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "OK");
            }
        }

        [Obsolete]
        async void QueryRequest()
        {
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

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "can't questy", "OK");
            }

        }
    }
}
