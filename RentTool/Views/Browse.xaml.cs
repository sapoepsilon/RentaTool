using System;
using System.Collections.Generic;
using System.Linq;
using RentTool.Models;
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

        void CollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            string selectedID = (e.CurrentSelection.FirstOrDefault() as CardTool)?.id;
            Navigation.PushAsync(new Views.ToolDetail(selectedID));
        }
    }
}
