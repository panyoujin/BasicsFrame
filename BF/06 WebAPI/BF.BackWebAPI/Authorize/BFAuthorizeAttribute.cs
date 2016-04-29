using BF.BackWebAPI.Models.Back;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Authorize
{
    public class BFAuthorizeAttribute : AuthorizeAttribute
    {
        private AuthorizationContext _httpContext;
        public bool IsLogin { get; set; }
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsAuthorized { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IsAuthorized = true;
            ApiResult<string> apiResult = new ApiResult<string> { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (filterContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            this._httpContext = (AuthorizationContext)filterContext;
            if (IsLogin)
            {
                if (UserInfo == null || UserInfo.ID <= 0)
                {
                    apiResult.code = ResultCode.CODE_ERROR_USER_NOT_LOGIN;
                    apiResult.msg = ResultMsg.CODE_ERROR_USER_NOT_LOGIN;
                    IsAuthorized = false;
                    
                    //httpContext.Response
                    //httpContext.Response = JsonHelper.SerializeObjectToWebApi(apiResult);
                    //返回异常
                }
            }
            if (IsAdmin&& (UserInfo == null || !UserInfo.IsAdmin))
            {
                apiResult.code = ResultCode.CODE_EXCEPTION;
                apiResult.msg = "无权访问！";
                IsAuthorized = false;
            }
            base.OnAuthorization(filterContext);
            if (!this.IsAuthorized)
            {
                ReturnAlert(filterContext, JsonHelper.SerializeObject(apiResult));
            }

        }

        /// <summary>
        /// 验证身份
        /// </summary>
        /// <param name="httpContext">Http上下文</param>
        /// <returns></returns>
        [Authorize]
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return this.IsAuthorized;
        }

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
                    return null;
                }
                var cacheUser = _httpContext.HttpContext.Cache.Get(SessionID);
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
                    _httpContext.HttpContext.Cache.Remove(SessionID);
                    _httpContext.HttpContext.Cache.Insert(SessionID, user);
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
                    _requestInfo = new RequestHelper(_httpContext.HttpContext.Request);
                }
                return _requestInfo;
            }
        }

        /// <summary>
        /// Js原生弹窗提示
        /// </summary>
        /// <param name="filterContext">AuthorizationContext</param>
        /// <param name="msg">提示信息</param>
        protected void ReturnAlert(ControllerContext filterContext, string msg)
        {
            filterContext.HttpContext.Response.Expires = -1;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            filterContext.HttpContext.Response.ContentType = "text/json";
            filterContext.HttpContext.Response.Write(msg);
            filterContext.HttpContext.Response.Flush();
            filterContext.HttpContext.Response.End();
        }
    }
}
