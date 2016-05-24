using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front.Response
{
    public class AppVersionResponse
    {
        public decimal Version { set; get; }
        public string InnerVersion { set; get; }
        public string TargetUrl { set; get; }
        public string UpdateContent { set; get; }
        public string UpdateStatus { set; get; }
        public decimal LowestVersion { set; get; }
    }
}