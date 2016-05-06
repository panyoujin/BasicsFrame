using System.ComponentModel;

namespace BF.Common.Enums
{
    /// <summary>
    /// 分享源类型 0:自定义; 1:养生品; 2:养生堂
    /// </summary>
    public enum Share_Source_Type
    {
        /// <summary>
        /// 自定义分享
        /// </summary>
        [Description("自定义分享")]
        Custom = 0,
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

    /// <summary>
    ///分享评论附件类型 0:分享; 1:评论
    /// </summary>
    public enum Share_Attmnt_Type
    {
        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share = 0,
        /// <summary>
        /// 评论
        /// </summary>
        [Description("评论")]
        Comment = 1,
    }

    /// <summary>
    /// 评论源类型  0:分享; 1:养生品; 2:养生堂
    /// </summary>
    public enum Comment_Source_Type
    {
        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share = 0,
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


    /// <summary>
    /// 评论类型 1.图文 2:赞
    /// </summary>
    public enum Comment_Type
    {
        /// <summary>
        /// 图文
        /// </summary>
        [Description("图文")]
        ImageText = 1,
        /// <summary>
        /// 赞
        /// </summary>
        [Description("赞")]
        Praise = 2,
    }
}
