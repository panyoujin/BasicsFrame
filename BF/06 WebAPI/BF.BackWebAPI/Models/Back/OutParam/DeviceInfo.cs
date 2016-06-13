using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.OutParam
{
    public class DeviceInfo
    {
        public int ID { set; get; }
        public string device_id { set; get; }
        public string device_identifier { set; get; }
        public string name { set; get; }
        public string device_ip { set; get; }
        public string router_id { set; get; }
        public string device_type { set; get; }
        public int MemberID { set; get; }
        public string CreationDate { set; get; }
        public string CreationUser { set; get; }
        public string StatusStr { set; get; }

    }
}