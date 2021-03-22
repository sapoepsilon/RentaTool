using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace RentTool.Views
{
    public partial class ToolDetail : ContentPage
    {
        public ToolDetail(string id)
        {
            InitializeComponent();
            string toolID = id;
            toolIdLabel.Text = toolID;
        }
    }
}
