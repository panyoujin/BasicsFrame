using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Enums
{
    
    /// <summary>
    /// 意见处理状态 1.待处理 2:已处理
    /// </summary>
    public enum FeedbackStatusEnum
    {
        /// <summary>
        /// 待处理 1
        /// </summary>
        [Description("待处理")]
        Wait = 1,
        /// <summary>
        /// 已处理 2
        /// </summary>
        [Description("已处理")]
        Complete = 2,
    }
}
