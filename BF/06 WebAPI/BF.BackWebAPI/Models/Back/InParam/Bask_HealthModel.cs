﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Back.InParam
{
    public class Bask_HealthModel
    {
        //string model_Name, string icoUrl = "", string imageUrl = "", string introduce = "", string describe = "", string remarks = "", int sort = 0, bool isBubble = false, int bubble_Time = 0, int bubble_Temperature = 0, int cook_Time = 0, int cook_Temperature = 0, bool is_Heat_Preservation = false, int heat_Preservation_Time = 0, int heat_Preservation_Temperature = 0, int removal_Chlorine_Time = 0, int final_Temperature = 0, bool isFerv = false
        public string Model_Name { get; set; }
        public string IcoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Introduce { get; set; }
        public string Describe { get; set; }
        public string Remarks { get; set; }
        public int Sort { get; set; }
        public bool IsBubble { get; set; }
        public int Bubble_Time { get; set; }
        public int Bubble_Temperature { get; set; }
        public int Cook_Time { get; set; }
        public int Cook_Temperature { get; set; }
        public bool Is_Heat_Preservation { get; set; }
        public int Heat_Preservation_Time { get; set; }
        public int Heat_Preservation_Temperature { get; set; }
        public int Removal_Chlorine_Time { get; set; }
        public int Final_Temperature { get; set; }
        public bool IsFerv { get; set; }


        public string WeChatUrl { get; set; }

        public int MID { get; set; }
    }
}