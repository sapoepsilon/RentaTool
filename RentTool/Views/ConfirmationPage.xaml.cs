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
        public string toolOfUser;
        string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private string toolID;
        ObservableCollection<Models.user> userInformation = new ObservableCollection<Models.user>();


        [Obsolete]

        public ConfirmationPage(string id)
        {
            InitializeComponent();
            this.toolID = id;
            QueryRequest();
           
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

                foreach (var toolOfUser in QueryObject.toolID)
                {
                    var userInfo = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("users")
                    .GetDocument(this.toolOfUser)
                    .GetAsync();
                    var getTheUser = userInfo.ToObject<Models.user>();

                    FirstName.Text = getTheUser.firstName;
                    LastName.Text = getTheUser.lastName;
                }
                
  

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }
        
    }
}
