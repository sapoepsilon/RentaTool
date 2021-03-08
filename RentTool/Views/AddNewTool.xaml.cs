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
    public partial class AddNewTool : ContentPage
    {
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string userId;
        public string IDTool;
   
        public AddNewTool()
        {
            InitializeComponent();
            
        }

        async void AddToolButton_Clicked(System.Object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            var savedfirebaseauth =
                JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                    ""));

            userId = savedfirebaseauth.User.LocalId;
            try
            {
                var addTool = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .AddAsync(new
                    {
                        toolName = toolName.Text,
                        toolPrice = toolPrice.Text,
                        toolDescription = toolDescription.Text,
                        toolPayment = toolPayment.Text,
                        toolAddress = toolAddress.Text,
                        UserId = userId,


                    });
                IDTool = addTool.Id;

               await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("users")
                        .GetDocument(userId)
                        .UpdateAsync("toolID", FieldValue.ArrayUnion(IDTool)); 



                
                 
                await App.Current.MainPage.DisplayAlert("Alert", "Your tool has been created with the tool id: " + IDTool, "Ok");
                




            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");

            }


        }


    }
    }
            

