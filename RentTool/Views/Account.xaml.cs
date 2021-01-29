using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RentTool
{
    public partial class Account : ContentPage
    {
        public Account()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddNewTool());
        }
    }

}
