using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class DeleteDerviceCodeResponse
    {
        public bool success { set; get; }
        public string shuihu_qr_code { set; get; }
        //public string wangguan_qr_code { set; get; }
    }
}