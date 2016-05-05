using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.Models.Request
{
    public class AddShareRequest
    {


        /// <summary>
        /// 源ID
        /// </summary>
        public int Source_ID { get; set; }

        /// <summary>
        /// 源类型0:自定义; 1:养生品; 2:养生堂
        /// </summary>
        public int Source_Type { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        public string ShareUrl { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string ShareTitle { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ShareContent { get; set; }
    }
}
