using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Xsl;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using RentTool.Controls;
using RentTool.Models;
using RentTool.ViewModels;
using RentTool.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentTool
{
    public partial class Browse : ContentPage
    {


        public Browse()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            MessagingCenter.Subscribe<AddNewTool>(this, "Refresh", (s) => { Refresh(); });
            MessagingCenter.Subscribe<EditTool>(this, "Refresh", (s) => { Refresh(); });

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
            }
            else
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

        private string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";


        private async void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            string search = searchBar.Text;
            try
            {
                var document = CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .WhereEqualsTo("toolName", search)
                    .GetAsync();

                var tool =  document.Result.ToObjects<toolQuery>();
                tools.ItemsSource = document.Result.Documents.ToString();
                
       
                
        
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
    }

    