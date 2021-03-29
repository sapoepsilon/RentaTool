using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using RentTool.Models;
using RentTool.ViewModels;
using Stripe;
using Stripe.Issuing;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static RentTool.Models.CardTool;

namespace RentTool.Views
{
    [DesignTimeVisible(false)]
    public partial class CreditCardPage : ContentPage
    {
        private Token stripeToken;
        private TokenService TokenService;
        private string toolID;
        private string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private string userEmail;


        private string stripeAPIKey =
            "pk_test_51IPy4cGrlGP6UBf685TcRd4arux8SmbLMDn5Rh7RytyEv3BRtkD5NOrMqCCLrn7zOoAmDoC7CA3ZQXFJfS3Gipe400oBatXkYI";

        public CreditCardPage(string toolID)
        {
            InitializeComponent();
            this.BindingContext = new CreditCardViewModel();
            this.toolID = toolID;
            displayPrice();
        }

        private async void Purchase_OnClicked(object sender, EventArgs e)
        {
            PaymentAsync();
            await Navigation.PushAsync( new Browse());

        }

        private async Task PaymentAsync()
        {
            bool isTrasacitonSuccess = false;

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            try
            {
                UserDialogs.Instance.ShowLoading("Payment is processing...");
                await Task.Run(async () =>
                {
                    var Token = CreateToken();

                    if (Token != null)
                    {
                        isTrasacitonSuccess = await makePayment();
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Card Credentials are invalid", null, "OK");
                    }
                });
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert(ex.Message, "Error", "Ok");
            }
            finally //If everything is correct navigate to the Browse page.
            {
                if (isTrasacitonSuccess)
                {
                    UserDialogs.Instance.Alert("Success", "Payment has been made", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }

        }

        private async void displayPrice()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            try
            {
                var ToolDocument = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(toolID)
                    .GetAsync();

                var getTool = ToolDocument.ToObject<Models.toolQuery>();
                Purchase.Text ="$" + getTool.toolPrice;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            
        }
        //get the payment
        private async Task<bool> makePayment()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            var savedfirebaseauth =
                JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                    ""));
            userEmail = savedfirebaseauth.User.Email;

            try
            {
                var ToolDocument = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(toolID)
                    .GetAsync();

                var getTool = ToolDocument.ToObject<Models.toolQuery>();
                long amount = long.Parse(getTool.toolPrice);
                Console.WriteLine(amount);
                try
                {
                    StripeConfiguration.SetApiKey(
                        "sk_test_51IPy4cGrlGP6UBf6tMB42W2ocKJriS9rw0V1l1SLEjfaIZGfACbpds2EsgGsckzSGmJJQVM9PtD3sv7S2St9RVpl007AHisqKY");
                    var chargeCreation = new ChargeCreateOptions
                    {
                        Amount = amount * 100,
                        Currency = "usd",
                        Source = stripeToken.Id,
                        ReceiptEmail = userEmail,
                        Capture = true
                    };

                    var service = new ChargeService();
                    Charge charge = service.Create(chargeCreation);
                    return true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    throw;
                }
     
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }

        //Create the token
        private string CreateToken()
        {

            try
            {
                StripeConfiguration.ApiKey = stripeAPIKey;
                var service = new ChargeService();

                var tokenOption = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = CardNumber.Text,
                        ExpMonth = long.Parse(CardExpirationDate.Text.Substring(0, 2)),
                        ExpYear = long.Parse(CardExpirationDate.Text.Substring(3, 2)),
                        Cvc = CardCVV.Text
                    }
                };
                TokenService = new TokenService();
                stripeToken = TokenService.Create(tokenOption);
                return stripeToken.Id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}