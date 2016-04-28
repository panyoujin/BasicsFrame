using BF.BackWebAPI.Models;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public string Login(string account, string password)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码不能为空！";
                return JsonHelper.SerializeObject(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("UserAccount", account);
            var user = DBBaseFactory.DALBase.QueryForObject<UserModel>("BackWeb_GetUserByLoginVoucher", dic);
            if (user == null || user.ID <= 0)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号不存在！";
                return JsonHelper.SerializeObject(apiResult);
            }
            if (user.UserPassword != password)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码错误！";
                return JsonHelper.SerializeObject(apiResult);
            }
            Login_Cache(user);
            apiResult.data = new { userName=user.UserName, ImageUrl=user.ImageUrl};
            return JsonHelper.SerializeObject(apiResult);
        }

        public string GetUserInfo()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            apiResult.data = UserInfo;
            return JsonHelper.SerializeObject(apiResult);
        }
    }
}