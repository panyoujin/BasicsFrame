using BF.BackWebAPI.Models.Front;
using BF.Common.DataAccess;
using BF.Common.Helper;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class BaseController : ApiController
    {
        public MemberInfo MemberInfo
        {
            get
            {
                var user = RequestInfo.UserInfo<MemberInfo>();
                if (user == null || user.ID <= 0)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("SessionID", RequestInfo.SessionID);
                    //从数据看获取
                    user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("FrontApi_GetMemberInfoByAccount", dic);
                    HttpContext.Current.Cache.Remove(RequestInfo.SessionID);
                    HttpContext.Current.Cache.Insert(RequestInfo.SessionID, user);
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
        public string Login_Cache(MemberInfo user)
        {
            #region 添加缓存
            var sessionID = Guid.NewGuid().ToString();
            HttpContext.Current.Cache.Remove(sessionID);
            HttpContext.Current.Cache.Insert(sessionID, user);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MemberID", user.ID);
            dic.Add("SessionID", sessionID);
            dic.Add("ModifyUser", user.Account);
            try
            {
                //更新数据库登录标识字段，这样一个帐号只能在一台机器常登录
                DBBaseFactory.DALBase.ExecuteNonQuery("FrontApi_UpdateSessionID", dic);
            }
            catch
            {

            }
            HttpCookie cook = new HttpCookie("CACHED_SESSION_ID", sessionID);
            HttpContext.Current.Response.AppendCookie(cook);
            #endregion
            return sessionID;
        }
        public void Update_Cache(MemberInfo user)
        {
            HttpContext.Current.Cache.Remove(RequestInfo.SessionID);
            HttpContext.Current.Cache.Insert(RequestInfo.SessionID, user);
        }

        public void Delete_Cache()
        {
            HttpContext.Current.Cache.Remove(RequestInfo.SessionID);
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