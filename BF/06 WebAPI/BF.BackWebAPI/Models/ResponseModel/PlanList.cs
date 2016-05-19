using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class PlanList
    {
        /// <summary>
        /// 
        /// </summary>
        public int Model_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sIntroduce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sIcoUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Plan_RGB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Plan_Time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Plan_TimeTicks { get; set; }
    }
}
