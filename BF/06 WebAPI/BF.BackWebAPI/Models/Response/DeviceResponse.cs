using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Response
{
    public class DeviceResponse
    {
        public string id { set; get; }
        public string device_identifier { set; get; }
        public string name { set; get; }
        public bool turned_on { set; get; }
        public int device_ip { set; get; }
        public int router_id { set; get; }
        public int house_id { set; get; }
        public string connectivity { set; get; }
        public int accumulated_usage_time { set; get; }
        public DateTime created_at { set; get; }
        public int death_qr_code { set; get; }
        public string device_type { set; get; }

        public string device_id
        {
            get
            {
                return this.id;
            }
        }
    }
}