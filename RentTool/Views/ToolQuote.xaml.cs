using System;
using System.Collections.Generic;
using Firebase.Auth;
using Plugin.CloudFirestore;
using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class ToolQuote : ContentPage
    {

        public string UserID;
        string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string toolQuery;
        private string toolID;

        [Obsolete]

        public ToolQuote(string id)
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

        private void RentButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.CreditCardPage(toolID));
        }

        DatePicker datePicker = new DatePicker
        {
            MinimumDate = new DateTime(2021, 1, 1),
            MaximumDate = new DateTime(2022, 12, 31),
            Date = new DateTime(2021, 1,1)
        };
    }
}


