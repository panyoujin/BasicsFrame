using BF.Common.Enums;
using System;
using System.Web;

namespace BF.BackWebAPI.Models.Request
{
    public class AddPlanRequest
    {
        public AddPlanRequest()
        {
            //int temp = 0;
            //int.TryParse(HttpContext.Current.Request.Form["Model_ID"], out temp);
            //this.Model_ID = temp;
            //DateTime tempTime = DateTime.MaxValue;
            //DateTime.TryParse(HttpContext.Current.Request.Form["Plan_Time"], out tempTime);
            //this.Plan_Time = tempTime;
        }

        /// <summary>
        /// 计划模式
        /// </summary>
        public int Model_ID { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime Plan_Time { get; set; }
    }



}
