using System.ComponentModel;

namespace BF.Common.Enums
{
    /// <summary>
    /// 分享源类型 1:自定义; 2:养生品; 3:养生堂
    /// </summary>
    public enum Share_Source_Type
    {
        /// <summary>
        /// 自定义分享
        /// </summary>
        [Description("自定义分享")]
        Custom = 1,
        /// <summary>
        /// 养生品
        /// </summary>
        [Description("养生品")]
        Model = 2,
        /// <summary>
        /// 养生堂
        /// </summary>
        [Description("养生堂")]
        Honyaradoh = 3,
    }

    /// <summary>
    ///分享评论附件类型 1:分享; 2:评论
    /// </summary>
    public enum Share_Attmnt_Type
    {
        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share = 1,
        /// <summary>
        /// 评论
        /// </summary>
        [Description("评论")]
        Comment = 2,
    }

    /// <summary>
    /// 评论源类型  1:分享; 2:养生品; 3:养生堂
    /// </summary>
    public enum Comment_Source_Type
    {
        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share = 1,
        /// <summary>
        /// 养生品
        /// </summary>
        [Description("养生品")]
        Model = 2,
        /// <summary>
        /// 养生堂
        /// </summary>
        [Description("养生堂")]
        Honyaradoh = 3,
    }


    /// <summary>
    /// 评论类型 1.图文（评论） 2:赞
    /// </summary>
    public enum Comment_Type
    {
        /// <summary>
        /// 图文（评论）
        /// </summary>
        [Description("图文（评论）")]
        ImageText = 1,
        /// <summary>
        /// 赞
        /// </summary>
        [Description("赞")]
        Praise = 2,
    }
}
