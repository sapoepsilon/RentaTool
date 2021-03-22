using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using RentTool.Models;

namespace RentTool.ViewModels
{
    //INotifyPropertyChanges - This interface is used to notify clients, typically binding clients (controls),
    // that a property value has changed in order to update them properly.
    // Source: https://almirvuk.blogspot.com/2016/12/xamarinforms-simple-mvvm-binding-example.html
    public class BrowseViewModel : INotifyPropertyChanged
    {

        //List that the items will be added!
        private ObservableCollection<CardTool> items;

        //Necessary - ?!
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CardTool> Items
        {
            get { return items; }
            set
            {
                items = value;
            }
        }


        public BrowseViewModel()
        {
            Items = new ObservableCollection<CardTool>() {
                new CardTool()
                {
                    name = "Drill",
                    image = "drill.jpg",
                    pricePerDay = 12f,
                    distance = 1.7f
                },
                  new CardTool()
                {
                    name = "Saw",
                    image = "saw.png",
                    pricePerDay = 15f,
                    distance = 4.2f
                },
                  new CardTool()
                {
                    name = "Pliers",
                    image = "pliers.jpg",
                    pricePerDay = 4.75f,
                    distance = 6.4f
                },
                  new CardTool()
                {
                    name = "Hammer",
                    image = "hammer.png",
                    pricePerDay = 15,
                    distance = 8.3f
                },new CardTool()
                {
                    name = "Drill",
                    image = "drill.jpg",
                    pricePerDay = 12f,
                    distance = 1.7f
                },
                  new CardTool()
                {
                    name = "Saw",
                    image = "saw.png",
                    pricePerDay = 15f,
                    distance = 4.2f
                },
                  new CardTool()
                {
                    name = "Pliers",
                    image = "pliers.jpg",
                    pricePerDay = 4.75f,
                    distance = 6.4f
                },
                  new CardTool()
                {
                    name = "Hammer",
                    image = "hammer.png",
                    pricePerDay = 15,
                    distance = 8.3f
                },new CardTool()
                {
                    name = "Drill",
                    image = "drill.jpg",
                    pricePerDay = 12f,
                    distance = 1.7f
                },
                  new CardTool()
                {
                    name = "Saw",
                    image = "saw.png",
                    pricePerDay = 15f,
                    distance = 4.2f
                },
                  new CardTool()
                {
                    name = "Pliers",
                    image = "pliers.jpg",
                    pricePerDay = 4.75f,
                    distance = 6.4f
                },
                  new CardTool()
                {
                    name = "Hammer",
                    image = "hammer.png",
                    pricePerDay = 15,
                    distance = 8.3f
                },
            };

        }
    }
}
