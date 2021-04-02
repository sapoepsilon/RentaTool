﻿using System;
using System.Collections.Generic;
using System.Linq;
using RentTool.Models;
using RentTool.ViewModels;
using RentTool.Views;
using Xamarin.Forms;

namespace RentTool
{
    public partial class Browse : ContentPage
    {

        public Browse()
        {
            
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            MessagingCenter.Subscribe<AddNewTool>(this, "Refresh", (s) => {
                Refresh();
                
            });
            MessagingCenter.Subscribe<EditTool>(this, "Refresh", (s) =>
            {
                Refresh();
            });

            // Connecting context of this page to the our View Model class
            BindingContext = new BrowseViewModel();
        }

        private void Refresh()
        {
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

        //protected override void OnAppearing() {
        //    base.OnAppearing();
        //    InitializeComponent();
        //    Console.Write("ITS SHOWING");
        //}
    }
}
