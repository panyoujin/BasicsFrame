using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Enums
{
    /// <summary>
    /// 问题类型 1.维修 2:换货 3:退货
    /// </summary>
    public enum AftermarketQuestionType
    {
        /// <summary>
        /// 维修
        /// </summary>
        [Description("维修")]
        Repair = 1,
        /// <summary>
        /// 换货
        /// </summary>
        [Description("换货")]
        Change = 2,
        /// <summary>
        /// 退货
        /// </summary>
        [Description("退货")]
        Retreat = 3,
    }
    /// <summary>
    /// 售后状态 1.待处理 2:申请不通过 3:处理中 4:完成
    /// </summary>
    public enum AftermarketStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Description("待处理")]
        Wait = 1,
        /// <summary>
        /// 申请不通过
        /// </summary>
        [Description("申请不通过")]
        No = 2,
        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Processing = 3,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Complete = 4,
    }
    /// <summary>
    /// 备注源类型
    /// </summary>
    public enum RemarksSourceTypeEnum
    {
        /// <summary>
        /// 售后
        /// </summary>
        [Description("售后")]
        Aftermarket = 1
    }
}
