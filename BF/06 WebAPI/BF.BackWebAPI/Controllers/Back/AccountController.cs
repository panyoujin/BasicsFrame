using BF.BackWebAPI.Models.Back;
using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.Front;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class AccountController : BackBaseController
    {
        [HttpPost]
        public HttpResponseMessage Login([FromBody]Login login)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(login.account) || string.IsNullOrWhiteSpace(login.password))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码不能为空！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("UserAccount", login.account);
            var user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("BackWeb_GetUserByLoginVoucher", dic);
            if (user == null || user.ID <= 0)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号不存在！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            user.IsAdmin = true;
            if (user.Passwd != login.password)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码错误！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            var sessionID = Login_Cache(user);
            apiResult.data = new { userName=user.Name, ImageUrl=user.ImageUrl, SessionID = sessionID };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpGet]
        public HttpResponseMessage GetUserInfo()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            apiResult.data = UserInfo;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

    }
}