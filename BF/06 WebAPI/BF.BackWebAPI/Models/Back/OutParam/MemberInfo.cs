using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Back.OutParam
{
    public class MemberInfo
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Account { set; get; }
        public int Age { set; get; }
        public string Sex { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string QQ { set; get; }
        public string ImageUrl { set; get; }
        public string IsAdmin { set; get; }
        public string StatusStr { set; get; }
        public string CreationDate { set; get; }

    }
}