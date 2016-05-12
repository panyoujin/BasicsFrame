using BF.BackWebAPI.Models.RequestModels;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class AftermarketController : BaseController
    {
        /// <summary>
        /// 添加售后
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAftermarket([FromBody]AddAftermarketRequest model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (model == null || string.IsNullOrWhiteSpace(model.ProductCode) || model.QuestionType <= 0 || string.IsNullOrWhiteSpace(model.QuestionDescribe))
            {
                throw new BusinessException("请填写完整数据在提交");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("QuestionType", model.QuestionType);
            dic.Add("ProductCode", model.ProductCode);
            dic.Add("QuestionDescribe", model.QuestionDescribe);
            //dic.Add("AftermarketStatus", (int)AftermarketStatus.Wait);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddAftermarket", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

    }
}