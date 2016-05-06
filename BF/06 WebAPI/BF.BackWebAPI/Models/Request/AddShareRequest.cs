using BF.Common.Enums;

namespace BF.BackWebAPI.Models.Request
{
    public class AddShareRequest
    {


        /// <summary>
        /// 源ID
        /// </summary>
        public int Source_ID { get; set; }

        private int _source_Type = -1;
        /// <summary>
        /// 源类型0:自定义; 1:养生品; 2:养生堂
        /// </summary>
        public int Source_Type
        {
            get
            {
                if (_source_Type < 0)
                {
                    _source_Type = (int)Share_Source_Type.Custom;
                }
                return _source_Type;
            }
            set { _source_Type = value; }
        }

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
    }

    public class Praise
    {
        public int Source_ID { get; set; }

        private int _source_Type = -1;
        /// <summary>
        /// 评论源类型  0:分享; 1:养生品; 2:养生堂
        /// </summary>
        public int Source_Type
        {
            get
            {
                if (_source_Type < 0)
                {
                    _source_Type = (int)Comment_Source_Type.Share;
                }
                return _source_Type;
            }
            set { _source_Type = value; }
        }
        
        public int Accept_Comment_User_ID { get; set; }
    }


    
}
