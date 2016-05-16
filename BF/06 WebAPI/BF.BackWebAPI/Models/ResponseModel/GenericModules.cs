using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class GenericModules
    {
        public string id { set; get; }
        public string device_identifier { set; get; }
        public string name { set; get; }
        public int device_ip { set; get; }
        public int router_id { set; get; }
        public string connectivity { set; get; }

        public string device_id
        {
            get
            {
                return this.id;
            }
        }
        public Basics basics { set; get; }

    }

    public class Basics
    {
        /// <summary>
        /// 0:开关  1:不知道  2:是否正在煮水  3:是否正在保温
        /// </summary>
        public List<int> bools { set; get; }
        /// <summary>
        /// mode0 - 0    设置40℃
        /// mode0 - 1    设置45℃
        /// mode0 - 2    设置50℃
        /// mode0 - 3    设置55℃
        /// mode0 - 4    设置60℃
        /// mode0 - 5    设置65℃
        /// mode0 - 6    设置70℃
        /// mode0 - 7    设置75℃
        /// mode0 - 8    设置80℃
        /// mode0 - 9    设置85℃
        /// </summary>
        public int mode { set; get; }
    }
}