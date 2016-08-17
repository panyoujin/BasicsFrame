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
            this.Param = HttpContext.Current.Request.Form["Param"];
            int.TryParse(HttpContext.Current.Request.Form["MID"], out temp);
            this.MID = temp;
            int.TryParse(HttpContext.Current.Request.Form["ModelType_ID"], out temp);
            this.ModelType_ID = temp;
        }
        //string model_Name, string icoUrl = "", string imageUrl = "", string introduce = "", string describe = "", string remarks = "", int sort = 0, bool isBubble = false, int bubble_Time = 0, int bubble_Temperature = 0, int cook_Time = 0, int cook_Temperature = 0, bool is_Heat_Preservation = false, int heat_Preservation_Time = 0, int heat_Preservation_Temperature = 0, int removal_Chlorine_Time = 0, int final_Temperature = 0, bool isFerv = false
        public string model_Name { get; set; }
        public string icoUrl { get; set; }
        public string imageUrl { get; set; }
        public string introduce { get; set; }
        public string describe { get; set; }
        public string remarks { get; set; }
        public int sort { get; set; }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public int IsCustom { get; set; }
        /// <summary>
        /// 指令参数值：100,100|0,0|0,0|0,0|0,0|100,50
        /// </summary>
        public string Param { get; set; }


        public string WeChatUrl { get; set; }

        public int MID { get; set; }

        public int ModelType_ID { get; set; }
    }
}
