using BF.BackWebAPI.Models.Front;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Encrypt;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class WeChartController : BaseController
    {
        /// <summary>
        /// 微信登录接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage WeChartLogin(string openid, string nickname)
        {
            string sessionID = "";
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(openid))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "openid不能为空！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("openid", openid);
            MemberInfo user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("Wechart_GetMemberInfoByOpenID", dic);
            if (user != null)
            {//已经存在绑定
                sessionID = Login_Cache(user);
            }
            else
            { //未绑定
                user = new Models.Front.MemberInfo();
                user.Account = "测试";
                user.Passwd = MD5Encrypt.Md5("123456");
                user.Name = nickname;
                user.openid = openid;
                user.nickname = nickname;

                dic.Clear();
                dic.Add("Account", user.Account);

                var userH = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("FrontApi_GetMemberInfoByAccount", dic);
                if (userH != null)
                {
                    apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                    apiResult.msg = "手机已存在！";
                    return JsonHelper.SerializeObjectToWebApi(apiResult);
                }
                dic.Add("Passwd", user.Passwd);
                dic.Add("Name", user.Name);
                dic.Add("openid", user.openid);
                dic.Add("nickname", user.nickname);
                int result = DBBaseFactory.DALBase.ExecuteNonQuery("Wechart_InsertMemberInfo", dic);
                if (result > 0)
                {
                    user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("Wechart_GetMemberInfoByOpenID", dic);
                    sessionID = Login_Cache(user);
                }
                else
                {
                    apiResult.code = ResultCode.CODE_EXCEPTION;
                    apiResult.msg = "新增用户失败！";
                    return JsonHelper.SerializeObjectToWebApi(apiResult);
                }
            }

            apiResult.data = new { Account = user.Account, Name = user.Name, ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "" : Global.AttmntUrl + user.ImageUrl, SessionID = sessionID };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 微信绑定账号接口
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage WeChartBaindAccount(string openid, string nickname)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(openid))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "openid不能为空！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("openid", openid);
            MemberInfo user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("Wechart_GetMemberInfoByOpenID", dic);
            if (user != null)
            {//已经存在绑定
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "该微信号已经绑定了其他账号！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            dic.Add("nickname", nickname);
            dic.Add("ModificationUser", MemberInfo.Account);
            dic.Add("MemberID", MemberInfo.ID);

            int result = DBBaseFactory.DALBase.ExecuteNonQuery("Wechart_UpdateMemberInfoBind", dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "绑定失败！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 微信取消绑定接口
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage WeChartDeleteBaindAccount(string openid)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (string.IsNullOrWhiteSpace(openid))
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "openid不能为空！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("openid", openid);

            dic.Add("ModificationUser", MemberInfo.Account);
            dic.Add("MemberID", MemberInfo.ID);

            int result = DBBaseFactory.DALBase.ExecuteNonQuery("Wechart_DeleteMemberInfoBind", dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_BUSINESS_ERROR;
                apiResult.msg = "取消绑定失败！";
                return JsonHelper.SerializeObjectToWebApi(apiResult);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
