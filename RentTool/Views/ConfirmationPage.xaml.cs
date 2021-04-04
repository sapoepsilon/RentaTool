using System;
using System.Collections.Generic;
using Firebase.Auth;
using Plugin.CloudFirestore;
using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class ConfirmationPage : ContentPage
    {
        public string UserID;
        string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private string toolID;

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
                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }

        
    }
}
