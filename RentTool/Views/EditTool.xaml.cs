using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
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
        public string IDTool;

        public EditTool(string toolId)
        {
            InitializeComponent();
            IDTool = toolId;
            DisplayAlert("alert",IDTool, "OK" );
            
                
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
                    .GetDocument(IDTool)
                    .SetAsync(new
                    {
                        toolName = toolName.Text,
                        toolPrice = toolPrice.Text,
                        toolDescription = toolDescription.Text,
                        toolPayment = toolPayment.Text,
                        toolAddress = toolAddress.Text,
                        UserId = userId,
                    }, true);


                await App.Current.MainPage.DisplayAlert("Alert", "Your tool has been updated with the tool id: ", "Ok");
                Navigation.PushAsync(new Account());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
            }
        }
    }
}