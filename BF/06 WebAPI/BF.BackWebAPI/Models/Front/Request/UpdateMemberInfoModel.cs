using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front.Request
{
    public class UpdateMemberInfoModel
    {
        public int ID { set; get; }
        public string Account { set; get; }
        public string Passwd { set; get; }
        public string Name { set; get; }
        public int Age { set; get; }
        public bool Sex { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string QQ { set; get; }
        public string ImageUrl { set; get; }

    }
}