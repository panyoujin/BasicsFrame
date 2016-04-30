using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class CommonModel
    {
        public CommonModel()
        {
            var temp = HttpContext.Current.Request.Form["modelID"];
            var tempID = 0;
            if(int.TryParse(temp, out tempID))
            {
                this.modelID = tempID;
            }
        }
        public int modelID { get; set; }
    }
}
