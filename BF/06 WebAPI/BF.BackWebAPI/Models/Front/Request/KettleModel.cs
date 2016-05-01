using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front.Request
{
    public class KettleModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Version { set; get; }
    }
}