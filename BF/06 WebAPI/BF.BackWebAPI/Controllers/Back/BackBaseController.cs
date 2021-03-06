﻿using BF.BackWebAPI.Models.Back;
using BF.BackWebAPI.Models.Front;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{

    public class BackBaseController : ApiController
    {

        public BackBaseController()
        {
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
                    user = DBBaseFactory.DALBase.QueryForObject<MemberInfo>("BackWeb_GetLoginUser", dic);
                    if (user != null)
                    {
                        user.IsAdmin = true;
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
            HttpContext.Current.Response.AppendCookie(cook);
            return sessionID;
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