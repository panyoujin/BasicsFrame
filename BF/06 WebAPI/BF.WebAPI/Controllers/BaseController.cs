﻿using BF.Common.Helper;
using BF.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BF.WebAPI.Controllers
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
                    //Dictionary<string, object> dic = new Dictionary<string, object>();
                    //dic.Add("SessionID", RequestInfo.SessionID);
                    ////从数据看获取
                    //user = DBBaseFactory.DALBase.QueryForObject<UserModel>("BackWeb_GetLoginUser", dic);
                    //HttpContext.Current.Cache.Remove(RequestInfo.SessionID);
                    //HttpContext.Current.Cache.Insert(RequestInfo.SessionID, user);
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
        public void Login_Cache(MemberInfo user)
        {
            #region 添加缓存
            var sessionID = Guid.NewGuid().ToString();
            HttpContext.Current.Cache.Remove(sessionID);
            HttpContext.Current.Cache.Insert(sessionID, user);
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("ID", user.ID);
            //dic.Add("SessionID", sessionID);
            //从数据看获取
            //try
            //{
            //    //更新数据库登录标识字段，这样一个帐号只能在一台机器常登录
            //    DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_UpdateLoginUserSessionID", dic);
            //}
            //catch
            //{

            //}
            HttpCookie cook = new HttpCookie("CACHED_SESSION_ID", sessionID);
            HttpContext.Current.Response.AppendCookie(cook);
            #endregion
        }
    }
}