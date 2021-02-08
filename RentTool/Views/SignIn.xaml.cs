using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RentTool
{
    public partial class SignIn : ContentPage
    {
        public SignIn()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MainContainerTabbedPage());
        }
    }
}
