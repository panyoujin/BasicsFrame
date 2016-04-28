using BF.BackWebAPI.Models.Back;
using BF.Common.DataAccess;
using BF.Common.Helper;
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
        private HttpContextBase _httpContext;
        [Authorize]
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            this._httpContext = httpContext;
            return true;
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
                var cacheUser = _httpContext.Cache.Get(SessionID);
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
                    _httpContext.Cache.Remove(SessionID);
                    _httpContext.Cache.Insert(SessionID, user);
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
                    _requestInfo = new RequestHelper(_httpContext.Request);
                }
                return _requestInfo;
            }
        }
    }
}
