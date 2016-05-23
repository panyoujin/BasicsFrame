using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Enums
{
    /// <summary>
    /// 评论源类型  1:分享; 2:养生品; 3:养生堂
    /// </summary>
    public enum Collection_Source_Type
    {
        /// <summary>
        /// 养生品
        /// </summary>
        [Description("养生品")]
        Model = 1,
        /// <summary>
        /// 养生堂
        /// </summary>
        [Description("养生堂")]
        Honyaradoh = 2,
    }
}
