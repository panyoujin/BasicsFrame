using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class InsertAdvertise
    {
        public int ID { set; get; }
        public string TypeCode { set; get; }
        public string Name { set; get; }
        public string Sort { set; get; }
        public string ImageUrl { set; get; }
        public string FullUrl { set; get; }
    }
}