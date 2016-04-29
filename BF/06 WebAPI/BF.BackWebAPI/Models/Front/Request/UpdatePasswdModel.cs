using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Front.Request
{
    public class UpdatePasswdModel
    {
        public string OldPasswd { set; get; }
        public string NewPasswd { set; get; }
    }
}