using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using BF.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF.WebAPI.Controllers
{
    public class MemberController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage Login(string account, string passwd)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(passwd))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码不能为空！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Account", account);
            var user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("FrontApi_GetMemberInfoByAccount", dic);
            if (user == null || user.ID <= 0)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号不存在！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            if (user.Passwd != passwd)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码错误！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Login_Cache(user);
            apiResult.data = new { Account = user.Account, ImageUrl = user.ImageUrl };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
