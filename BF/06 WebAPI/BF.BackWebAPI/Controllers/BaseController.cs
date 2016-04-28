using BF.BackWebAPI.Models;
using BF.Common.DataAccess;
using BF.Common.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers
{

    public class BaseController : Controller
    {

        public BaseController()
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
                var cacheUser = HttpContext.Cache.Get(SessionID) as UserModel;
                if (cacheUser == null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("SessionID", SessionID);
                    //从数据看获取
                    cacheUser = DBBaseFactory.DALBase.QueryForObject<UserModel>("BackWeb_GetLoginUser", dic);
                    HttpContext.Cache.Remove(SessionID);
                    HttpContext.Cache.Insert(SessionID, cacheUser);
                }
                return cacheUser;
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
    }
}