using BF.Common.Enums;
using System.Web;

namespace BF.BackWebAPI.Models.Request
{
    public class AddShareRequest
    {
        public AddShareRequest()
        {
            int temp = 0;
            int.TryParse(HttpContext.Current.Request.Form["Source_ID"], out temp);
            this.Source_ID = temp;
            if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.Form["Source_Type"]) || int.TryParse(HttpContext.Current.Request.Form["Source_Type"], out temp))
            {
                temp = (int)Share_Source_Type.Custom;
            }
            this.Source_Type = temp;
            this.ShareUrl = HttpContext.Current.Request.Form["ShareUrl"] ?? "";
            this.ShareTitle = HttpContext.Current.Request.Form["ShareTitle"] ?? "";
            this.ShareContent = HttpContext.Current.Request.Form["ShareContent"] ?? "";
        }
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



}
