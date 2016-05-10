using BF.Common.Enums;
using System.Web;

namespace BF.BackWebAPI.Models.Request
{
    public class AddAftermarketRequest
    {
        public AddAftermarketRequest()
        {
            int temp = 0;
            if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.Form["QuestionType"]) || !int.TryParse(HttpContext.Current.Request.Form["QuestionType"], out temp))
            {
                temp = (int)AftermarketQuestionType.Repair;
            }
            this.QuestionType = temp;
            this.ProductCode = HttpContext.Current.Request.Form["ProductCode"] ?? "";
            this.QuestionDescribe = HttpContext.Current.Request.Form["QuestionDescribe"] ?? "";
        }
        
        /// <summary>
        /// 源类型1:自定义; 2:养生品; 3:养生堂
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        /// 源Url
        /// </summary>
        public string ProductCode { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public string QuestionDescribe { get; set; }
    }



}
