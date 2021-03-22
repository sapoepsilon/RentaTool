using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;
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
                    Console.WriteLine(tool.toolName);
                    Console.WriteLine(tool.pictureUrl);
                    Console.WriteLine(tool.toolPrice);

                    Items.Add(new CardTool()
                    {
                        name = tool.toolName,
                        image = tool.pictureUrl,
                        pricePerDay = tool.toolPrice,
                        distance = 10.0f
                    });
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}
