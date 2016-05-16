using BF.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{

    public class ShareListResponse
    {
        /// <summary>
        /// 分享ID
        /// </summary>
        public int ShareID { get; set; }

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
        //public string ShareTitle { get; set; }

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
        /// <summary>
        /// 我赞的数量,用于判断我是否赞过
        /// </summary>
        public int MyPraiseCount { get; set; }

        public string AttachUrls { get; set; }

        public string BaseAttmntUrl { get; set; }
        private List<string> _imgUrlList;
        /// <summary>
        /// 图片地址
        /// </summary>
        public List<string> ImgUrlList
        {
            get
            {
                _imgUrlList = new List<string>();
                if (!string.IsNullOrWhiteSpace(AttachUrls))
                {
                    var urls = AttachUrls.Split(',');
                    foreach (var item in urls)
                    {
                        if(item.IndexOf("http://")==0|| item.IndexOf("https://") == 0)
                        {
                            _imgUrlList.Add(item);
                        }
                        else
                        {
                            _imgUrlList.Add(BaseAttmntUrl + item);
                        }
                    }
                }
                return _imgUrlList;
            }
        }

        //public DateTime CreationDate { get; set; }

        public long CreationDateTicks { get; set; }
        //public long _creationDateTicks
        //{
        //    get
        //    {
        //        return this.CreationDate.UnixDateTicks();
        //    }
        //}
    }
}