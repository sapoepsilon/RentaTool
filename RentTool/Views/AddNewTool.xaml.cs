﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using Plugin.FirebaseStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RentTool.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using RentTool.Models;

namespace RentTool
{
    public partial class AddNewTool : ContentPage
    {
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        public string userId;
        FileResult photo;
        public string IDTool;
        // ObservableCollection<ImagePath> listPath = new ObservableCollection<ImagePath>();


        public AddNewTool()
        {
            InitializeComponent();
        }

        async void AddToolButton_Clicked(System.Object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            var savedfirebaseauth =
                JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                    ""));

            userId = savedfirebaseauth.User.LocalId;
            try
            {

               
              var address = toolAddress.Text;
                    var locations = await Geocoding.GetLocationsAsync(address);

                    var location = locations?.FirstOrDefault();
                   
                    
                        var addToolAddress = await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("tools")
                            .AddAsync(new
                            {
                                toolLongtitude = location.Longitude,
                                toolLatitude = location.Latitude
                            });
                        IDTool = addToolAddress.Id;
                    
                
              
            

            await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(IDTool)
                    .SetAsync(new 
                    {
                        toolName = toolName.Text,
                        toolPrice = toolPrice.Text,
                        toolDescription = toolDescription.Text,
                        toolPayment = toolPayment.Text,
                        toolAddress = toolAddress.Text,
                        UserId = userId,
                    }, true);
                

                await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(IDTool)
                    .SetAsync(new {toolID = IDTool}, true);

                await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("users")
                    .GetDocument(userId)
                    .UpdateAsync("toolID", FieldValue.ArrayUnion(IDTool));


                await App.Current.MainPage.DisplayAlert("Alert",
                    "Your tool has been created with the tool id: " + IDTool, "Ok");

                var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child(photo.FullPath);
                var stream = await photo.OpenReadAsync();

                await reference.PutStreamAsync(stream);
                var url = await reference.GetDownloadUrlAsync();

                await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("tools")
                    .GetDocument(IDTool)
                    .SetAsync(new {pictureUrl = url.AbsoluteUri}, true);

                Navigation.PushAsync(new Account());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }

        private async void ToolPhotos_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
        }

        private async void TakePic_OnClicked(object sender, EventArgs e)
        {
            try
            {
                photo = await MediaPicker.CapturePhotoAsync();
                String filepath = photo.FullPath;
                LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                imageOfTheTool = null;
                return;
            }

            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            imageOfTheTool.Source = newFile;
        }

        private async void AddPic_OnClicked(object sender, EventArgs e)
        {
            try
            {
                photo = await MediaPicker.PickPhotoAsync();
                String filepath = photo.FullPath;
                LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }
    }
}