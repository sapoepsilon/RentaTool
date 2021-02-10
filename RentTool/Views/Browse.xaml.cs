using System;
using System.Collections.Generic;
using RentTool.ViewModels;
using Xamarin.Forms;

namespace RentTool
{
    public partial class Browse : ContentPage
    {
        Iauth _auth;
        public Browse()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            // Connecting context of this page to the our View Model class
            BindingContext = new BrowseViewModel();
            _auth = DependencyService.Get<Iauth>();
        }


        private void SignOut_Clicked(object sender, EventArgs e)
        {
            var signOut = _auth.SignOut();

            if (signOut)
            {
                Application.Current.MainPage = new SignIn(); 
            }
        }
    }
}
