using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class HealthModel
    {
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
    }
}
