using BF.BackWebAPI.Models.Back;
using BF.BackWebAPI.Models.Front;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BF.BackWebAPI.Authorize
{
    public class BFAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private HttpActionContext _httpContext;
        public bool IsLogin { get; set; }
        public bool IsAdmin { get; set; }

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


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            this._httpContext = actionContext;
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (IsLogin)
            {

                if (UserInfo == null || UserInfo.ID <= 0)
                {
                    apiResult.code = ResultCode.CODE_ERROR_USER_NOT_LOGIN;
                    apiResult.msg = ResultMsg.CODE_ERROR_USER_NOT_LOGIN;
                    actionContext.Response = JsonHelper.SerializeObjectToWebApi(apiResult);
                }

            }
        }

        public MemberInfo UserInfo
        {
            get
            {
                var user = RequestInfo.UserInfo<MemberInfo>();
                if (user == null || user.ID <= 0 && !string.IsNullOrWhiteSpace(RequestInfo.SessionID))
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("SessionID", RequestInfo.SessionID);
                    //从数据看获取
                    user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("FrontApi_GetMemberInfoByAccount", dic);
                    if (user != null)
                    {
                        //user.IsAdmin = true;
                        HttpContext.Current.Cache.Remove(RequestInfo.SessionID);
                        HttpContext.Current.Cache.Insert(RequestInfo.SessionID, user);
                    }
                }
                if (user == null || user.ID <= 0)
                {
                    throw new NotLoginException(ResultMsg.CODE_ERROR_USER_NOT_LOGIN);
                }
                return user;
            }
        }

    }
}
