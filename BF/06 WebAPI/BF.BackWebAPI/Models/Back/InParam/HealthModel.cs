using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class HealthModel
    {
        public HealthModel()
        {
            this.model_Name = HttpContext.Current.Request.Form["model_Name"] ?? "";
            this.icoUrl = HttpContext.Current.Request.Form["icoUrl"] ?? "";
            this.imageUrl = HttpContext.Current.Request.Form["imageUrl"] ?? "";
            this.introduce = HttpContext.Current.Request.Form["introduce"] ?? "";
            this.describe = HttpContext.Current.Request.Form["describe"] ?? "";
            this.remarks = HttpContext.Current.Request.Form["remarks"] ?? "";
            this.WeChatUrl = HttpContext.Current.Request.Form["WeChatUrl"] ?? "";
            int temp = 0;
            int.TryParse(HttpContext.Current.Request.Form["sort"], out temp);
            this.sort = temp;
            int.TryParse(HttpContext.Current.Request.Form["bubble_Time"], out temp);
            this.bubble_Time = temp;
            int.TryParse(HttpContext.Current.Request.Form["bubble_Temperature"], out temp);
            this.bubble_Temperature = temp;
            int.TryParse(HttpContext.Current.Request.Form["cook_Time"], out temp);
            this.cook_Time = temp;
            int.TryParse(HttpContext.Current.Request.Form["cook_Temperature"], out temp);
            this.cook_Temperature = temp;
            int.TryParse(HttpContext.Current.Request.Form["heat_Preservation_Time"], out temp);
            this.heat_Preservation_Time = temp;
            int.TryParse(HttpContext.Current.Request.Form["heat_Preservation_Temperature"], out temp);
            this.heat_Preservation_Temperature = temp;
            int.TryParse(HttpContext.Current.Request.Form["removal_Chlorine_Time"], out temp);
            this.removal_Chlorine_Time = temp;
            int.TryParse(HttpContext.Current.Request.Form["final_Temperature"], out temp);
            this.final_Temperature = temp;
            int.TryParse(HttpContext.Current.Request.Form["MID"], out temp);
            this.MID = temp;
            int.TryParse(HttpContext.Current.Request.Form["ModelType_ID"], out temp);
            this.ModelType_ID = temp;
            var booltemp = false;
            bool.TryParse(HttpContext.Current.Request.Form["isBubble"], out booltemp);
            this.isBubble = booltemp;
            bool.TryParse(HttpContext.Current.Request.Form["is_Heat_Preservation"], out booltemp);
            this.is_Heat_Preservation = booltemp;
            bool.TryParse(HttpContext.Current.Request.Form["isFerv"], out booltemp);
            this.isFerv = booltemp;
        }
        //string model_Name, string icoUrl = "", string imageUrl = "", string introduce = "", string describe = "", string remarks = "", int sort = 0, bool isBubble = false, int bubble_Time = 0, int bubble_Temperature = 0, int cook_Time = 0, int cook_Temperature = 0, bool is_Heat_Preservation = false, int heat_Preservation_Time = 0, int heat_Preservation_Temperature = 0, int removal_Chlorine_Time = 0, int final_Temperature = 0, bool isFerv = false
        public string model_Name { get; set; }
        public string icoUrl { get; set; }
        public string imageUrl { get; set; }
        public string introduce { get; set; }
        public string describe { get; set; }
        public string remarks { get; set; }
        public int sort { get; set; }
        public bool isBubble { get; set; }
        public int bubble_Time { get; set; }
        public int bubble_Temperature { get; set; }
        public int cook_Time { get; set; }
        public int cook_Temperature { get; set; }
        public bool is_Heat_Preservation { get; set; }
        public int heat_Preservation_Time { get; set; }
        public int heat_Preservation_Temperature { get; set; }
        public int removal_Chlorine_Time { get; set; }
        public int final_Temperature { get; set; }
        public bool isFerv { get; set; }


        public string WeChatUrl { get; set; }

        public int MID { get; set; }

        public int ModelType_ID { get; set; }
    }
}
