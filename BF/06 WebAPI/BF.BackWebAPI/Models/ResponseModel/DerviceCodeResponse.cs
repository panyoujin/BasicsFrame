using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class DerviceCodeResponse
    {
        public bool success { set; get; }
        public string qr_code { set; get; }
    }
}