using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class MyDevices
    {
        public string uuid { set; get; }
        public string device_id { set; get; }
        public string device_identifier { set; get; }
        public string name { set; get; }
        public bool turned_on { set; get; }

        public string connectivity { set; get; }

        public string death_qr_code { set; get; }
        public string device_type { set; get; }

        public bool is_default { set; get; }

        public int device_ip { set; get; }
        public int router_id { set; get; }
    }
}