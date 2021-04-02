using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using RentTool.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentTool.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTool : ContentPage
    {
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string userId;
        public string tool;
 
        

        public EditTool(string tool)
        {
            InitializeComponent(); 
            this.tool = tool;
            getTool();
        }

        private async void EditToolButton_Clicked(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            var savedfirebaseauth =
                JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                    ""));
            
            userId = savedfirebaseauth.User.LocalId;
            
            try
            {

                await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(tool)
                    .SetAsync(new
                    {
                        toolName = toolName.Text,
                        toolPrice = toolPrice.Text,
                        toolDescription = toolDescription.Text,
                        toolPayment = toolPayment.Text,
                        toolAddress = toolAddress.Text,
                        UserId = userId,
                    }, true);

                
                MessagingCenter.Send<EditTool>(this, "Refresh");

                Navigation.PushAsync(new Account());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
            }
        }

        public async void getTool()
        {
            var toolQuery = await CrossCloudFirestore.Current
                .Instance
                .GetCollection("tools")
                .GetDocument(tool)
                .GetAsync();

            var toolDocument = toolQuery.ToObject<toolQuery>();
            toolImage.Source = toolDocument.pictureUrl;
            toolName.Text = toolDocument.toolName;
            toolDescription.Text = toolDocument.toolDescription;
            toolAddress.Text = toolDocument.toolAddress;
            toolPrice.Text = toolDocument.toolPrice;


            if (toolDocument.isAvailable == false)
            {
                toolAvailibility.IsToggled = false;
            }
            else
            {
                toolAvailibility.IsToggled = true;
            }
        }
        
        private async void ToolAvailibility_OnToggled(object sender, ToggledEventArgs e)
        {
            if (toolAvailibility.IsToggled == true)
            {
                try
                {
                    await CrossCloudFirestore.Current
                        .Instance
                        .Collection("tools")
                        .Document(tool)
                        .UpdateAsync(new { isAvailable = true });

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                }
            }

            else { 
                try
                {
                    await CrossCloudFirestore.Current
                        .Instance
                        .Collection("tools")
                        .Document(tool)
                        .UpdateAsync(new { isAvailable = false });

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                }}
        }
    }
}