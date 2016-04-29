using BF.BackWebAPI.Models.Front;
using BF.BackWebAPI.Models.Front.Request;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class MemberController : BaseController
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="account"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 注册接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Register([FromBody]RegisterModel param)
        {
            ApiResult<bool> apiResult = new ApiResult<bool>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(param.Account) || string.IsNullOrWhiteSpace(param.Passwd))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号或密码不能为空！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Account", param.Account);
            dic.Add("Passwd", param.Passwd);
            var user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("FrontApi_GetMemberInfoByAccount", dic);
            if (user != null)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "帐号已存在！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("FrontApi_InsertMemberInfo", dic);
            if (result > 0)
            {
                apiResult.data = true;
            }
            else
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "保存数据失败！";
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 更新资料接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateMemberInfo([FromBody]UpdateMemberInfoModel param)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", MemberInfo.ID);
            dic.Add("Account", param.Account);
            dic.Add("Passwd", param.Passwd);
            dic.Add("Name", param.Name);
            dic.Add("Age", param.Age);
            dic.Add("Sex", param.Sex);
            dic.Add("Email", param.Email);
            dic.Add("Phone", param.Phone);
            dic.Add("QQ", param.QQ);
            dic.Add("ImageUrl", param.ImageUrl);
            dic.Add("ModifyUser", MemberInfo.Account);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("FrontApi_UpdateMemberInfo", dic);
            if (result > 0)
            {
                MemberInfo user = new MemberInfo() { Account = param.Account, ID = MemberInfo.ID, Passwd = param.Passwd, Phone = param.Phone, Name = param.Name, Email = param.Email, QQ = param.QQ, ImageUrl = param.ImageUrl };
                Update_Cache(user);
                apiResult.data = new { Account = user.Account, ImageUrl = user.ImageUrl };
            }
            else
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "保存数据失败！";
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 修改密码接口
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdatePasswd([FromBody]UpdatePasswdModel param)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", MemberInfo.ID);
            dic.Add("ModifyUser", MemberInfo.Account);
            dic.Add("OldPasswd", param.OldPasswd);
            dic.Add("NewPasswd", param.NewPasswd);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("FrontApi_UpdateMemberInfoPasswd", dic);
            if (result > 0)
            {
                apiResult.data = true;
            }
            else
            {
                apiResult.code = ResultCode.CODE_EXCEPTION;
                apiResult.msg = "修改密码失败！";
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
