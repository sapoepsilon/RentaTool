using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class SignUp : ContentPage
    {
        Iauth _auth;
        public SignUp()
        {
            InitializeComponent();
            _auth = DependencyService.Get<Iauth>();
        }

         async void SignUpClicked(object sender, EventArgs e)
        {
            var user = _auth.SignUpWithEmailAndPassword(EmailInput.Text, PasswordInput.Text);

            if (user != null)
            {
                await DisplayAlert("success", "New User Created", "Ok");

                var signOut = _auth.SignOut();

                if (signOut)
                {
                    Application.Current.MainPage = new SignIn();
                }
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong, please try again", "Ok");

                {

                }
            }
        }
    }
}
