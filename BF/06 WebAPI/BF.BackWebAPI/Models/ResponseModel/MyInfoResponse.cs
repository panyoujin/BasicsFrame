﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class MyInfoResponse
    {
        public int uuid { set; get; }
        public string Account { set; get; }
        public string Email { set; get; }
        //public string Passwd { set; get; }
        public string ImageUrl { set; get; }
        public string Name { set; get; }
        public string QQ { set; get; }
        public string openid { set; get; }
        public string nickname { set; get; }
    }
}