using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentTool.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPassword : ContentPage
    {
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";

        

        public ResetPassword()
        {
            InitializeComponent();
        }

        async void Reset_Clicked(System.Object Sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            try
            {
                await authProvider.SendPasswordResetEmailAsync(UserLoginEmail.Text);
                Navigation.PushAsync(new SignIn());
                    await App.Current.MainPage.DisplayAlert("Success", "Reset link has been sent to your email, please check your email", "OK");

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "email can't be found" , "Try Again");
                }
            }


        }
    }


