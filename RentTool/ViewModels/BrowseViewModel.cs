using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Firebase.Auth;
using Plugin.CloudFirestore;
using RentTool.Models;

namespace RentTool.ViewModels
{
    //INotifyPropertyChanges - This interface is used to notify clients, typically binding clients (controls),
    // that a property value has changed in order to update them properly.
    // Source: https://almirvuk.blogspot.com/2016/12/xamarinforms-simple-mvvm-binding-example.html
    public class BrowseViewModel : INotifyPropertyChanged
    {

        public string WebApiKey = "AIzaSyAUum5OozKcO7mXvgnXIQ7PLTC8vdmXMcI";

        //List that the items will be added!
        private ObservableCollection<CardTool> items;

        //Necessary - ?!
        public event PropertyChangedEventHandler PropertyChanged;

        //public ObservableCollection<CardTool> Items
        //{
        //    get { return items; }
        //    set
        //    {
        //        items = value;
        //    }
        //}

        public static ObservableCollection<CardTool> Items = new ObservableCollection<CardTool>
         {
         };

        public static ObservableCollection<CardTool> itemTools
        {
            get { return Items; }

        }

        [Obsolete]
        public BrowseViewModel()
        {
            QueryRequest();
            Items = new ObservableCollection<CardTool>();
            
            //Items = new ObservableCollection<CardTool>() {
            //    new CardTool()
            //    {
            //        name = "Drill",
            //        image = "drill.jpg",
            //        pricePerDay = 12f,
            //        distance = 1.7f
            //    },
            //      new CardTool()
            //    {
            //        name = "Saw",
            //        image = "saw.png",
            //        pricePerDay = 15f,
            //        distance = 4.2f
            //    },
            //      new CardTool()
            //    {
            //        name = "Pliers",
            //        image = "pliers.jpg",
            //        pricePerDay = 4.75f,
            //        distance = 6.4f
            //    },
            //      new CardTool()
            //    {
            //        name = "Hammer",
            //        image = "hammer.png",
            //        pricePerDay = 15,
            //        distance = 8.3f
            //    },new CardTool()
            //    {
            //        name = "Drill",
            //        image = "drill.jpg",
            //        pricePerDay = 12f,
            //        distance = 1.7f
            //    },
            //      new CardTool()
            //    {
            //        name = "Saw",
            //        image = "saw.png",
            //        pricePerDay = 15f,
            //        distance = 4.2f
            //    },
            //      new CardTool()
            //    {
            //        name = "Pliers",
            //        image = "pliers.jpg",
            //        pricePerDay = 4.75f,
            //        distance = 6.4f
            //    },
            //      new CardTool()
            //    {
            //        name = "Hammer",
            //        image = "hammer.png",
            //        pricePerDay = 15,
            //        distance = 8.3f
            //    },new CardTool()
            //    {
            //        name = "Drill",
            //        image = "drill.jpg",
            //        pricePerDay = 12f,
            //        distance = 1.7f
            //    },
            //      new CardTool()
            //    {
            //        name = "Saw",
            //        image = "saw.png",
            //        pricePerDay = 15f,
            //        distance = 4.2f
            //    },
            //      new CardTool()
            //    {
            //        name = "Pliers",
            //        image = "pliers.jpg",
            //        pricePerDay = 4.75f,
            //        distance = 6.4f
            //    },
            //      new CardTool()
            //    {
            //        name = "Hammer",
            //        image = "hammer.png",
            //        pricePerDay = 15,
            //        distance = 8.3f
            //    },
            //};

        }

        [Obsolete]
        public async void QueryRequest()
        {
            Console.WriteLine("YAYYYYYYYYYYYYYYYY");
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

                Console.WriteLine("mmmmmmmmmmmm");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }

            //try
            //{
            //    var document = await CrossCloudFirestore.Current
            //        .Instance
            //        .GetCollection("tools")
            //        .GetAsync();

            //    Console.WriteLine(document);

            //}
            //catch (Exception ex)
            //{
            //    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            //}
        }
    }
}
