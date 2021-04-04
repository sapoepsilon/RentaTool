using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;
using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class ConfirmationPage : ContentPage
    {
       
        string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private string toolID;
        private string UserID;
      

        [Obsolete]

        public ConfirmationPage(string id)
        {
            InitializeComponent();
            this.toolID = id;
            QueryRequest();
           

        }

        public ConfirmationPage(string toolID, object v)
        {
            this.toolID = toolID;
           
        }

        [Obsolete]
        public async void QueryRequest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                var document = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(toolID)
                    .GetAsync();
                   

                var QueryObject = document.ToObject<Models.toolQuery>();

                ToolName.Text = QueryObject.toolName;
                ToolImage.Source = QueryObject.pictureUrl;

                UserID = QueryObject.UserId;
                //DisplayAlert("Message", UserID, "OK");

                try
                {
                    var idOfUser = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("users")
                    .GetDocument(UserID)
                    .GetAsync();
                    var QueryObjectUser = idOfUser.ToObject<Models.user>();

                    FirstName.Text = QueryObjectUser.firstName;
                    LastName.Text = QueryObjectUser.lastName;
                    PhoneNumber.Text = QueryObjectUser.phone;
                    Zip.Text = QueryObjectUser.zip;
                    

                }

                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "OK");
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }
        
    }
}
