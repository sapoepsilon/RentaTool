using System;
using Xamarin.Forms;

namespace RentTool.Models
{
    public class user
    {

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string creditCardNumber { get; set; }
        public string creditCardCvv { get; set; }
        public string creditCardExpiration { get; set; }
        public string[] toolID { get; set; }
        public user()
        {
        }
    }
}
