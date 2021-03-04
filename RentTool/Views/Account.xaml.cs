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
        public string token;
        public string user;
        public string toolID;

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
                token = savedfirebaseauth.FirebaseToken;
                user = savedfirebaseauth.User.Email;
                QueryRequest();

               

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }

        void ChangeClicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Views.ChangePassword());
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

         void UpdateClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Update());
        }

        [Obsolete]
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

              

               if (QueryObject.toolID != null)
               {

                   String toolName;

               foreach (var toolsOfUser in QueryObject.toolID)
               {

                   var toolID = await CrossCloudFirestore.Current
                       .Instance
                       .GetCollection("tools")
                       .GetDocument(toolsOfUser)
                       .GetAsync();

                   var getTheToolName = toolID.ToObject<tool>();



                   tools.Text= getTheToolName.toolName + " $" + getTheToolName.toolPrice; 
                  
                   
               }

               


               }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "OK");
            }

        }
    }
}
