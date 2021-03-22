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
            CollectionView collectionV = sender as CollectionView;
            //collectionV.SelectedItem = null;

                string previous = (e.PreviousSelection.FirstOrDefault() as CardTool)?.id;
                string selectedID = (e.CurrentSelection.FirstOrDefault() as CardTool)?.id;

                

            if (String.IsNullOrEmpty(selectedID))
            {
                return;
            } else
            {
                Navigation.PushAsync(new Views.ToolDetail(selectedID));
                collectionV.ClearValue(CollectionView.SelectedItemProperty);
            }
                

        }
    }
}
