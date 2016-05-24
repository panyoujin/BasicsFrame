using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.ResponseModel
{
    public class PlanGroupList
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

        private string _iocUrl;
        /// <summary>
        /// 
        /// </summary>
        public string sIcoUrl
        {
            get
            {
                if (_iocUrl == null || _iocUrl.IndexOf("http://") == 0 || _iocUrl.IndexOf("https://") == 0)
                {

                    return _iocUrl;
                }
                return Global.AttmntUrl + _iocUrl;
            }
            set
            {
                _iocUrl = value;
            }
        }

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
