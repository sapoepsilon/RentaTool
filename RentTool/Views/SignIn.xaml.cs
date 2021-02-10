using System;
using System.Collections.Generic;
using RentTool.Views;
using Xamarin.Forms;

namespace RentTool
{
    public partial class SignIn : ContentPage
    {
        Iauth _auth;
        public SignIn()
        {
            InitializeComponent();
            _auth = DependencyService.Get<Iauth>();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MainContainerTabbedPage());
        }

         void SignUpClicked(object sender, EventArgs e)
         {
             var signOut = _auth.SignOut();

             if (signOut)
             {
                 Application.Current.MainPage = new MainPage();
             }
         }

        async void LoginClicked(object sender, EventArgs e)
        {
            string token = await _auth.LogInWithEmailAndPassword(EmailInput.Text, PasswordInput.Text);
            if (token != String.Empty)
            {
                Application.Current.MainPage = new Browse();
            }
            else
            {
                await DisplayAlert("Authentification Failed", "Email or Password are incorrect", "Ok");
            }
        }
        
    }
}
