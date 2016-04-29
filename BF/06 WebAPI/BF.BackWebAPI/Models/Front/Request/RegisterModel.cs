using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front.Request
{
    public class RegisterModel
    {
        public string Account { set; get; }
        public string Passwd { set; get; }
    }
}