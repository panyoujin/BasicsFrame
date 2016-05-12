using BF.Common.Enums;
using System.Web;

namespace BF.BackWebAPI.Models.RequestModels
{
    public class AddAftermarketRequest
    {
        public AddAftermarketRequest()
        {
            //int temp = 0;
            //if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.Form["QuestionType"]) || !int.TryParse(HttpContext.Current.Request.Form["QuestionType"], out temp))
            //{
            //    //temp = (int)AftermarketQuestionType.Repair;
            //}
            //this.QuestionType = temp;
            //this.ProductCode = HttpContext.Current.Request.Form["ProductCode"] ?? "";
            //this.QuestionDescribe = HttpContext.Current.Request.Form["QuestionDescribe"] ?? "";
        }

        /// <summary>
        /// 问题类型 1.维修 2:换货 3:退货
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string QuestionDescribe { get; set; }
    }



}
