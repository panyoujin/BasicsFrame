using BF.Common.CustomException;
using BF.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BF.Common.Helper
{
    public class RequestHelper
    {
        public RequestHelper()
        {
            Request = HttpContext.Current.Request;
        }
        public RequestHelper(HttpRequest request)
        {
            Request = request;
        }

        public RequestHelper(HttpRequestBase requestMessage)
        {
            this.MvcRequestMessage = requestMessage;
        }
        public RequestHelper(HttpRequestMessage requestMessage)
        {
            this.APIRequestMessage = requestMessage;
        }
        /// <summary>
        /// 请求信息
        /// </summary>
        public HttpRequestMessage APIRequestMessage { get; set; }
        /// <summary>
        /// 请求信息
        /// </summary>
        public HttpRequestBase MvcRequestMessage { get; set; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public HttpRequest Request { get; set; }


        /// <summary>
        /// 缓存使用的session
        /// </summary>
        public string SessionID
        {
            get
            {
                return GetHeaderListToValue("CACHED_SESSION_ID");
                //return User.Identity.GetUserId();
            }
        }
        public T UserInfo<T>(bool isDB=false) where T : class, new()
        {

            if (string.IsNullOrWhiteSpace(SessionID))
            {
                throw new NotLoginException("用户未登录！");
            }
            var cacheUser = HttpContext.Current.Cache.Get(SessionID);
            T user = default(T);
            if (cacheUser != null)
            {
                user = cacheUser as T;
            }
            else if(isDB)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("SessionID", SessionID);
                //从数据看获取
                user = DBBaseFactory.DALBase.QueryForObject<T>("FrontApi_GetMemberInfoByAccount", dic);
                if (user != null)
                {

                    HttpContext.Current.Cache.Remove(SessionID);
                    HttpContext.Current.Cache.Insert(SessionID, user);
                }
            }
            return user;
        }

        /// <summary>
        /// 获得url参数数组（主要用于静态化）
        /// </summary>
        /// <param name="queryString">参数字符串</param>
        /// <returns>参数数组</returns>
        public string[] GetParameterList(string queryString)
        {
            //RequestMessage
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] list = queryString.Split('_');
                return list;
            }
            return null;
        }

        public string RequestIP
        {
            get
            {
                return GetIP();
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetIP()
        {
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            string userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //否则直接读取REMOTE_ADDR获取客户端IP地址
            if (!string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = userHostAddress.Split(',')[0].Trim();
            }
            else
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 获得请求头参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>参数值</returns>
        public string GetHeaderListToValue(string key)
        {

            if (!string.IsNullOrEmpty(key))
            {
                if (APIRequestMessage != null)
                {
                    if (APIRequestMessage.Headers.Where(h => h.Key.ToLower() == key.ToLower()).Count() > 0)
                    {
                        var values = APIRequestMessage.Headers.GetValues(key);
                        if (values != null && values.Count() > 0)
                        {
                            var value = "";
                            foreach (var str in values)
                            {
                                value += str;
                            }
                            return value;
                        }
                    }
                }
                else if (MvcRequestMessage != null)
                {
                    var values = MvcRequestMessage.Headers.GetValues(key);
                    if (values != null && values.Count() > 0)
                        return values.FirstOrDefault();
                }
                else if (Request != null)
                {
                    var values = Request.Headers.GetValues(key);
                    if (values != null && values.Count() > 0)
                        return values.FirstOrDefault();
                }
            }
            return GetCookieListToValue(key);
        }

        /// <summary>
        /// 获得请求头参数
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>参数值</returns>
        public string GetCookieListToValue(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                HttpCookieCollection values = null;

                if (APIRequestMessage != null)
                {
                    var values2 = APIRequestMessage.Headers.GetCookies();

                    if (values2 != null && values2.Count() > 0 && values2.FirstOrDefault() != null && values2.FirstOrDefault()[key] != null)
                    {
                        return values2.FirstOrDefault()[key].Value;
                    }
                }
                else if (MvcRequestMessage != null)
                {
                    values = MvcRequestMessage.Cookies;
                }
                else if (Request != null)
                {
                    values = Request.Cookies;
                }
                if (values != null)
                {
                    var cookie = values.Get(key);
                    if (cookie != null)
                    {
                        return cookie.Value;
                    }
                }
            }
            return null;
        }
    }
}
