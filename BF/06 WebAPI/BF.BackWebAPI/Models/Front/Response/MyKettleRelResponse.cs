using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front.Response
{
    public class MyKettleRelResponse
    {
        public int ID { set; get; }
        public string MemberID { set; get; }
        public string KettleID { set; get; }
        public bool Default { set; get; }
        public string CreationUser { set; get; }
        public DateTime CreationDate { set; get; }
    }
}