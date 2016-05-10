using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Enums
{
    /// <summary>
    /// 状态 1:待执行 2:已执行 3:已过期
    /// </summary>
    public enum PlanStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Description("待处理")]
        Wait = 1,
        /// <summary>
        /// 已执行
        /// </summary>
        [Description("已执行")]
        Complete = 2,
        /// <summary>
        /// 已过期
        /// </summary>
        [Description("已过期")]
        Expired = 3,
    }
}
