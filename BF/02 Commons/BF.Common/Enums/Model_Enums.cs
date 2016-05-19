using System.ComponentModel;

namespace BF.Common.Enums
{
    /// <summary>
    /// 类型：0.系统提供;1.自定义
    /// </summary>
    public enum Model_Types
    {
        /// <summary>
        /// 系统定义
        /// </summary>
        [Description("系统定义")]
        System =0,
        /// <summary>
        /// 自定义
        /// </summary>
        [Description("自定义")]
        Custom =1,
    }

    /// <summary>
    /// 状态：0.公开;1.私密主要是给自定义的模式使用
    /// </summary>
    public enum Model_Status
    {
        /// <summary>
        /// 公开
        /// </summary>
        [Description("公开")]
        Public = 0,
        /// <summary>
        /// 私密
        /// </summary>
        [Description("私密")]
        Private = 1,
    }
}
