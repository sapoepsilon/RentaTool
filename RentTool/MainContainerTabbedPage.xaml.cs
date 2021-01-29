using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentTool
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainContainerTabbedPage : TabbedPage
    {
        public MainContainerTabbedPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
    }
}
