using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentTool
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Debugger.IsAttached)
                Xamarin.Essentials.Preferences.Clear();

            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")))
            {
                MainPage = new NavigationPage(new MainContainerTabbedPage());
            }
            else
            {
                MainPage = new NavigationPage(new SignIn());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
