using System;
using System.Collections.Generic;

namespace RentTool.Models
{
    public class toolQuery
    {
        private string absoltureURI = "absoluteUri";
        public string toolName { get; set; }
        public string toolPrice { get; set; }
        public string toolPricetoolDescription { get; set; }
        public string toolPayment { get; set; }
        public string toolAddress { get; set; }
        public string toolID { get; set; }
        public string pictureUrl { get; set; }

        public toolQuery()
        {
        }
    }
}