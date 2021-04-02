using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//namespace RentTool
//{
//    [XamlCompilation(XamlCompilationOptions.Compile)]
//    public partial class MainContainerTabbedPage : TabbedPage
//    {
//        public MainContainerTabbedPage()
//        {
//            NavigationPage.SetHasNavigationBar(this, false);
//            InitializeComponent();
//        }
//    }
//}

namespace RentTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainContainerTabbedPage : TabbedPage
    {
        public MainContainerTabbedPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BarBackgroundColor = Color.DarkGoldenrod;
            BarTextColor = Color.Wheat;
            //(new NavigationPage(new CampaignPage()) { Title = "Campaign", Icon = "tab.png", BackgroundColor = Color.Black });
            Children.Add((new NavigationPage(new Browse()) { Title = "Browse" }));
            Children.Add((new NavigationPage(new Account()) { Title = "Account" }));
        }
        
    }
}


