using BF.BackWebAPI.Models.Front;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using Newtonsoft.Json;
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
                LogHelper.Info(string.Format("BaseController开始:{0}", RequestInfo.SessionID));
                var user = RequestInfo.UserInfo<MemberInfo>();
                if (user == null || user.ID <= 0)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("SessionID", RequestInfo.SessionID);
                    //从数据看获取

                    user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("FrontApi_GetMemberInfoByAccount", dic);
                    if (user != null)
                    {
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
                LogHelper.Info(string.Format("更新数据库登录标识字段开始："));
                //更新数据库登录标识字段，这样一个帐号只能在一台机器常登录
                var count = DBBaseFactory.DALBase.ExecuteNonQuery("FrontApi_UpdateSessionID", dic);
                LogHelper.Info(string.Format("更新数据库登录标识字段开始：{0}", count));
            }
            catch (Exception ex)
            {
                LogHelper.Info(string.Format("更新数据库登录标识字段失败：", ex.Message));
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


        /// <summary>
        /// 设置页码大小
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        public void SetPageSize(int page, int pageSize, ref int startSize, ref int endSize)
        {
            if (page <= 0)
            {
                page = CommonConstant.PAGE;
            }
            if (pageSize <= 0)
            {
                pageSize = CommonConstant.PAGE_SIZE;
            }
            startSize = (page > 1 ? (page - 1) * pageSize : 0);
            endSize = pageSize;

        }

        /// <summary>
        /// 换取幻腾接口的access_token
        /// </summary>
        public string Access_Token
        {
            get
            {
                string token = Global.APPInfo.Access_Token;
                if (string.IsNullOrEmpty(token))
                {
                    string address = string.Format("https://huantengsmart.com/oauth2/token?grant_type=refresh_token&refresh_token={1}", Global.APPInfo.Refresh_Token);

                    string returnStr = HttpRequestHelper.Request(address, "POST", 10);
                    if (!string.IsNullOrEmpty(returnStr))
                    {
                        BF.BackWebAPI.Controllers.HTSmartController.Access_TokenJson tokenJson = JsonConvert.DeserializeObject<BF.BackWebAPI.Controllers.HTSmartController.Access_TokenJson>(returnStr);
                        if (tokenJson != null)
                        {
                            Global.APPInfo.Access_Token = tokenJson.access_token;
                            Global.APPInfo.Expires_in = tokenJson.expires_in;
                            Global.APPInfo.Created_at = tokenJson.created_at;
                            Global.APPInfo.Token_Type = tokenJson.token_type;
                            Global.APPInfo.Refresh_Token = tokenJson.refresh_token;

                        }
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("ID", Global.APPInfo.ID);
                        dic.Add("Access_Token", Global.APPInfo.Access_Token);
                        dic.Add("Expires_in", Global.APPInfo.Expires_in);
                        dic.Add("Created_at", Global.APPInfo.Created_at);
                        dic.Add("Token_Type", Global.APPInfo.Token_Type);
                        dic.Add("Refresh_Token", Global.APPInfo.Refresh_Token);

                        DBBaseFactory.DALBase.ExecuteNonQuery("HTSmart_UpdateAPPInfoToken", dic);
                        token = Global.APPInfo.Access_Token;
                    }
                }
                return token;
            }
        }
    }
}