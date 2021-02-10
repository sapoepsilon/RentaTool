using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentTool
{
    public partial class App : Application
    {
        private Iauth _auth;
        public App()
        {
            InitializeComponent();
            _auth = DependencyService.Get<Iauth>();

            if (_auth.IsSignIn())
            {
                MainPage = new Browse();
            }
            else
            {
                MainPage = new MainPage();
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
