using System;
using System.Collections.Generic;
using RentTool.ViewModels;
using Xamarin.Forms;

namespace RentTool
{
    public partial class Browse : ContentPage
    {

        public Browse()
        {
            
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            // Connecting context of this page to the our View Model class
            BindingContext = new BrowseViewModel();
        }

    }
}
