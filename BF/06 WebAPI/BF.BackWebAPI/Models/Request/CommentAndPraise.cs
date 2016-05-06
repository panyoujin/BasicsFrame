using BF.Common.Enums;

namespace BF.BackWebAPI.Models.Request
{
    public class CommentAndPraise
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
        public string Comment_Content { get; set; }
        public int Accept_Comment_User_ID { get; set; }
    }
}
