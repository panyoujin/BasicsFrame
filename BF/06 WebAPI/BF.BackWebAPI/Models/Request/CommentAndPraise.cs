using BF.Common.Enums;
using System.Web;

namespace BF.BackWebAPI.Models.Request
{
    public class CommentAndPraise
    {
        public CommentAndPraise()
        {
            int temp = 0;
            int.TryParse(HttpContext.Current.Request.Form["Source_ID"], out temp);
            this.Source_ID = temp;
            if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.Form["Source_Type"]) || int.TryParse(HttpContext.Current.Request.Form["Source_Type"], out temp))
            {
                temp = (int)Share_Source_Type.Custom;
            }
            this.Source_Type = temp;
            this.Comment_Content = HttpContext.Current.Request.Form["Comment_Content"] ?? "";
            int.TryParse(HttpContext.Current.Request.Form["Accept_Comment_User_ID"], out temp);
            this.Accept_Comment_User_ID = temp;
        }
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
