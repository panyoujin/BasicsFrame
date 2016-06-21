using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.OutParam
{
    public class AdvertiseType
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Code { set; get; }
        public DateTime OperatioinDate { set; get; }

    }
}