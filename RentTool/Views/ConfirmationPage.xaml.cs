using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class ConfirmationPage : ContentPage
    {
       
        string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private string toolID;
        private string UserID;
        private double date;
        private string toolName;
        private string userNumber;



        [Obsolete]

        public ConfirmationPage(string id, double date)
        {
            InitializeComponent();
            this.toolID = id;
            this.date = date;
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

                UserID = QueryObject.UserId;
                
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
                    DatePeriod.Text = "days:" + date.ToString();
                    double totalAmount = date * double.Parse(QueryObject.toolPrice);
                    TotalAmount.Text = "$" + totalAmount.ToString();
                    Zip.Text = QueryObject.toolAddress;
                    

                    toolName = QueryObject.toolName;
                    userNumber = QueryObjectUser.phone;
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

        private async void SMS_OnClicked(object sender, EventArgs e)
        {
            string messageText = "Hello, I just rented your " + toolName +
                ", could I just pick it up right now? Thanks.";
            try
            {
                var message = new SmsMessage(messageText, new[] { userNumber });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                DisplayAlert("error", ex.StackTrace, "Ok");
            }
            catch (Exception ex)
            {
                DisplayAlert("error", ex.StackTrace, "Ok");
            }
        }

    }
}
