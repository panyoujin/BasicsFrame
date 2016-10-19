using BF.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BF.BackWebAPI.Models.ResponseModel
{
    
    public class MyMessageResponse
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentID { get; set; }

        /// <summary>
        /// 评论用户ID
        /// </summary>
        public int Comment_User_ID { get; set; }

        private string _user_Image;
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Comment_User_Image
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_user_Image))
                {

                    if (_user_Image.IndexOf("http://") == 0 || _user_Image.IndexOf("https://") == 0)
                    {
                        return _user_Image;
                    }
                    else
                    {
                        return Global.AttmntUrl + _user_Image;
                    }

                }
                return _user_Image;
            }
            set
            {
                _user_Image = value;
            }
        }


        /// <summary>
        /// 被评论的用户ID
        /// </summary>
        public int Accept_Comment_User_ID { get; set; }
        

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string User_Name { get; set; }

        public long CreationDateTicks { get; set; }

        public string Comment_Content { get; set; }
        public int Comment_Type { get; set; }
    }
}