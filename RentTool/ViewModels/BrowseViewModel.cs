using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Firebase.Auth;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using RentTool.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RentTool.ViewModels
{
    //INotifyPropertyChanges - This interface is used to notify clients, typically binding clients (controls),
    // that a property value has changed in order to update them properly.
    // Source: https://almirvuk.blogspot.com/2016/12/xamarinforms-simple-mvvm-binding-example.html
    public class BrowseViewModel : INotifyPropertyChanged
    {
        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";
        private ObservableCollection<CardTool> items;
        private double miles;
        private string userAddress;
        public string UserID;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CardTool> Items
        {
            get { return items; }
            set
            {
                items = value;
            }
        }

        [Obsolete]
        public BrowseViewModel()
        {
            getUsetZip();
            QueryRequest();
            Items = new ObservableCollection<CardTool>() {
        };

        }

        [Obsolete]
        async private void getUsetZip()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth =
                    JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",
                        ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                UserID = savedfirebaseauth.User.LocalId;

                var userDocument = await CrossCloudFirestore.Current
                    .Instance
                    .GetCollection("users")
                    .GetDocument(UserID)
                    .GetAsync();

                var QueryObject = userDocument.ToObject<Models.user>();
                userAddress = QueryObject.zip;

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        [Obsolete]
        public async void QueryRequest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            try
            {
                var group = await CrossCloudFirestore.Current
                                     .Instance
                                     .CollectionGroup("tools")
                                     .GetAsync();

                var yourModels = group.ToObjects<toolQuery>();
                
                foreach (var tool in yourModels)
                {
                    // 1) Grab user's location (lat, long)

                    try
                    {
                        var locationsUser = await Geocoding.GetLocationsAsync(userAddress);
                        var locationUser = locationsUser?.FirstOrDefault();

                        var locationsTool = await Geocoding.GetLocationsAsync(tool.toolAddress);
                        var locationTool = locationsTool?.FirstOrDefault();

                        if (locationUser != null)
                        {
                            Console.WriteLine($"User zip code: {userAddress}");
                            Console.WriteLine($"Latitude: {locationTool.Latitude}, Longitude: {locationTool.Longitude}, Altitude: {locationTool.Altitude}");
                            Console.WriteLine("miles:");
                            miles = Location.CalculateDistance(locationUser, locationTool, DistanceUnits.Miles);
                            Console.WriteLine(miles);
                        }
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        // Feature not supported on device
                        Console.WriteLine("NOT WORKING FOR THIS DEVICE");
                    }
                    catch (Exception ex)
                    {
                        // Handle exception that may have occurred in geocoding
                        Console.WriteLine("SOMETHING HAPPENED WITH GEOLOCATION");
                    }



                    // 2) Grab tool's location (lat, long)

                    // 3) Calculate distance and reference to the new CardTool

                    Items.Add(new CardTool()
                    {
                        id = tool.toolID,
                        name = tool.toolName,
                        image = tool.pictureUrl,
                        pricePerDay = tool.toolPrice,
                        distance = miles
                    }) ;
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        
    }
}
