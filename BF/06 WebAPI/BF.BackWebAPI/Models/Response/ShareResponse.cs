using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.Response
{
    
    public class ShareResponse
    {
        /// <summary>
        /// 分享ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int User_ID { get; set; }

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

        /// <summary>
        /// 赞数量
        /// </summary>
        public int PraiseCount { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string User_Name { get; set; }
    }
}