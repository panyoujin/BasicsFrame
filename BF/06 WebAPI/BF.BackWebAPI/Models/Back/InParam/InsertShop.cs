using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class InsertShop
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public decimal Price { set; get; }
        public decimal NowPrice { set; get; }
        public int Sort { set; get; }
        public string OpenationDate { set; get; }
        public int Enable { set; get; }
        public int OnShelf { set; get; }

        public string ImageUrl { set; get; }
        public string FullUrl { set; get; }
        public string ContentUrl { set; get; }
    }
}