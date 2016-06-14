using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.OutParam
{
    public class ModelType
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string TypeDescribe { set; get; }
        public string ImageUrl { set; get; }
        public string FullUrl { set; get; }
        public int TypeSort { set; get; }
        public DateTime OperatioinDate { set; get; }

    }
}