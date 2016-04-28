using BF.BackWebAPI.Models.Back;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers.Back
{

    public class BackBaseController : Controller
    {

        public BackBaseController()
        {
        }
        /// <summary>
        /// 缓存使用的session
        /// </summary>
        public string SessionID
        {
            get
            {
                return RequestInfo.GetHeaderListToValue("CACHED_SESSION_ID");
                //return User.Identity.GetUserId();
            }
        }

        public UserModel UserInfo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SessionID))
                {
                    throw new NotLoginException("用户未登录！");
                    return null;
                }
                var cacheUser = HttpContext.Cache.Get(SessionID);
                UserModel user = null;
                if (cacheUser != null)
                {
                    user = cacheUser as UserModel;
                    if (user != null && user.ID > 0)
                    {
                        return user;
                    }
                }
                else
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("SessionID", SessionID);
                    //从数据看获取
                    user = DBBaseFactory.DALBase.QueryForObject<UserModel>("BackWeb_GetLoginUser", dic);
                    HttpContext.Cache.Remove(SessionID);
                    HttpContext.Cache.Insert(SessionID, user);
                }
                return user;
            }
        }

        private RequestHelper _requestInfo;
        public RequestHelper RequestInfo
        {
            get
            {
                if (_requestInfo == null)
                {
                    _requestInfo = new RequestHelper(Request);
                }
                return _requestInfo;
            }
        }


        /// <summary>
        /// 会员登录，添加信息到缓存中,如果有就替换
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void Login_Cache(UserModel user)
        {
            #region 添加缓存
            var sessionID = Guid.NewGuid().ToString();
            HttpContext.Cache.Remove(sessionID);
            HttpContext.Cache.Insert(sessionID, user);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", user.ID);
            dic.Add("SessionID", sessionID);
            //从数据看获取
            try
            {
                //更新数据库登录标识字段，这样一个帐号只能在一台机器常登录
                DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateLoginUserSessionID", dic);
            }
            catch
            {

            }
            HttpCookie cook = new HttpCookie("CACHED_SESSION_ID", sessionID);
            HttpContext.Response.AppendCookie(cook);
            #endregion
        }

        /// <summary>
        /// 获取开始页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public int GetStartSize(int page, int pageSize)
        {
            return (page > 1 ? (page - 1) * pageSize : 0);
        }

        /// <summary>
        /// 将异常写入日志中
        /// </summary>
        /// <param name="errorContext"></param>
        protected override void OnException(ExceptionContext errorContext)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_EXCEPTION, msg = ResultMsg.CODE_EXCEPTION };
            string url = errorContext.HttpContext.Request.RawUrl;
            string postParameter = string.Empty;
            if (errorContext.HttpContext.Request.RequestType == "POST")
            {
                postParameter += "\r\n";
                foreach (string key in errorContext.HttpContext.Request.Form.AllKeys)
                {
                    postParameter += string.Format("\"{0}\":{1};", key, errorContext.HttpContext.Request.Form[key]);
                }
                postParameter += "\r\n";
            }
            string errMsg = string.Format("请求URL：{0},IP:{1},请求类型:{2}{3}", url, RequestInfo.RequestIP, errorContext.HttpContext.Request.RequestType, postParameter);
            LogHelper.Info(errMsg);
            if (errorContext.Exception is NotLoginException)
            {
                apiResult.code = ResultCode.CODE_ERROR_USER_NOT_LOGIN;
                apiResult.msg = ResultMsg.CODE_ERROR_USER_NOT_LOGIN;
            }
            Response.Write(JsonHelper.SerializeObjectToWebApi(apiResult));
            Response.End();
            base.OnException(errorContext);
        }
    }
}