using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Firebase.Auth;
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

            QueryRequest();
            Items = new ObservableCollection<CardTool>() {
        };

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
                    //Example to Calculate distance between 2 locations
                    //Location boston = new Location(42.358056, -71.063611);
                    //Location sanFrancisco = new Location(37.783333, -122.416667);
                    //double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
                    //Console.WriteLine(miles);

                    // 1) Grab user's location (lat, long)


                    // 2) Grab tool's location (lat, long)

                    // 3) Calculate distance and reference to the new CardTool

                    Items.Add(new CardTool()
                    {
                        id = tool.toolID,
                        name = tool.toolName,
                        image = tool.pictureUrl,
                        pricePerDay = tool.toolPrice,
                        distance = 10.0f
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
