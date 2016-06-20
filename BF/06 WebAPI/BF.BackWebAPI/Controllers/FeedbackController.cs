using BF.BackWebAPI.Models.RequestModels;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class FeedbackController : BaseController
    {
        /// <summary>
        /// 添加售后
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddFeedback()
        {
            string content = HttpContext.Current.Request.Form["QuestionContent"];
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS, data = true };
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new BusinessException("请填写意见再提交");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("QuestionDescribe", content);
            //dic.Add("FeedbackStatus", (int)FeedbackStatusEnum.Wait);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("FrontWeb_AddFeedback", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

    }
}