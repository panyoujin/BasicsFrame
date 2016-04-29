using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.WebAPI.Models
{
    public class MemberInfo
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string Passwd { set; get; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string ImageUrl { get; set; }
    }
}