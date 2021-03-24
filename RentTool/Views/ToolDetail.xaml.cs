using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;
using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class ToolDetail : ContentPage

    {
        public string UserID;
        string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string toolQuery;
        private string toolID;

        [Obsolete]
        public ToolDetail(string id)
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
                ToolPrice.Text = "$" + QueryObject.toolPrice;
                ToolDescription.Text =  QueryObject.toolDescription;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
                
            
        }
    }
}




        

