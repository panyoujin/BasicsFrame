using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.Request;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class AftermarketController : BaseController
    {
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="source_ID"></param>
        /// <param name="source_Type">收藏源类型 1.模式;2.文章;</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAftermarket([FromBody]AddAftermarketRequest model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("QuestionType", model.QuestionType);
            dic.Add("ProductCode", model.ProductCode);
            dic.Add("QuestionDescribe", model.QuestionDescribe);
            dic.Add("AftermarketStatus", (int)AftermarketStatus.Wait);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddCollection", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        
    }
}