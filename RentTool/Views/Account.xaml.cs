﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;
using RentTool.Views;

namespace RentTool
{
    public partial class Account : ContentPage
    {
        public string UserID;
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string token;
        public string user;
        public string toolID;
        ObservableCollection<Models.toolQuery> toolList = new ObservableCollection<Models.toolQuery>();
        private bool _canClose = true;

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


        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new AddNewTool());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
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
        public async void QueryRequest()
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
                zip.Text = QueryObject.zip;

                foreach (var toolsOfUser in QueryObject.toolID)
                {
                    var idOfTool = await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("tools")
                        .GetDocument(toolsOfUser)
                        .GetAsync();


                    var getTheToolName = idOfTool.ToObject<toolQuery>();


                    toolList.Add(new toolQuery
                    {
                        toolName = getTheToolName.toolName, toolPrice = "$" + getTheToolName.toolPrice,
                        pictureUrl = getTheToolName.pictureUrl, toolID = getTheToolName.toolID
                       
                    });
                }

                toolName.ItemsSource = toolList;


                //toolNames.ItemSelected += (sender, e) =>
                //{
//				if (e.SelectedItem == null) return;
//				Debug.WriteLine("Selected: " + e.SelectedItem);
//				((ListView)sender).SelectedItem = null; // de-select the row
//			};

                // If using ItemSelected
//			listView.ItemSelected += (sender, e) => {
//				if (e.SelectedItem == null) return;
//				Debug.WriteLine("Selected: " + e.SelectedItem);
//				((ListView)sender).SelectedItem = null; // de-select the row
//			};

                //Padding = new Thickness(0, 20, 0, 0);
                //Content = toolView;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var tool =  ((ImageButton) sender).BindingContext as string;
            var answer = await App.Current.MainPage.DisplayAlert("Delete tool?",
                "Are you sure you want to delete this tool?",
                "Yes", "No");
            
            if (answer == true)
            {
                try
                {
                    
                    await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("tools")
                        .GetDocument(tool)
                        .DeleteAsync();
                    toolList.Remove(toolList.Where(i => i.toolID == tool).Single());

                    await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("users")
                        .GetDocument(UserID)
                        .UpdateAsync("toolID", FieldValue.ArrayRemove(tool));
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                }

                await App.Current.MainPage.DisplayAlert("Alert", "The tool has been deleted", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Smart choice", "This tool might bring you some buck!",
                    "Yay!");
            }
        }

        private async void ToolName_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                var tool = (toolQuery)e.SelectedItem;
        
                try
                {
                    Navigation.PushAsync(new EditTool(tool.toolID));
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");

                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }
    }
}