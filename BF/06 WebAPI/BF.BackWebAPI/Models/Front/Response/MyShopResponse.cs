using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front
{
    public class MyShopResponse
    {
        public int uuid { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public string Url { get; set; }
        public string ContentUrl { set; get; }
        public string ImageUrl { set; get; }
        public decimal Price { get; set; }
        public decimal NowPrice { get; set; }
    }
}