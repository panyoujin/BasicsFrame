﻿using BF.BackWebAPI.Models.Front;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.Models;
using BF.Common.SQLAnalytical;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BF.BackWebAPI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CacheSqlConfig.Instance.SqlConfigPath = Server.MapPath("/bin/SqlConfig");

            Global.InitSettings();//加载附件服务器地址
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            string url = Request.RawUrl;
            string postParameter = string.Empty;
            if (Request.RequestType == "POST")
            {
                postParameter += "\r\n";
                foreach (string key in Request.Form.AllKeys)
                {
                    try
                    {
                        postParameter += string.Format("\"{0}\":{1};", key, Request.Form[key]);
                    }
                    catch (HttpRequestValidationException ex)
                    {
                        postParameter += string.Format("\"{0}\":{1};", key, "此参数因安全问题无法获取");
                    }
                }
                postParameter += "\r\n";
            }
            RequestHelper rh = new RequestHelper(Request);
            string errMsg = string.Format("请求URL：{0},IP:{1},请求类型:{2}{3},SessionID:{4}", url, RequestHelper.GetIP(), Request.RequestType, postParameter, rh.SessionID);
            LogHelper.Info(errMsg);
        }
        protected void Application_AuthenticateRequest()
        {

            //a.GenerateUserIdentityAsync()
            //var user = await UserManager.FindAsync("UserName", "Password");
            //var claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.Name, "jimmy.pan"));
            //claims.Add(new Claim(ClaimTypes.Role, "Users"));
            //var identity = new ClaimsIdentity(claims, "MyClaimsLogin");
            //ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            //HttpContext.Current.User = principal;
        }
        
    }

    public class Global
    {
        /// <summary>
        /// 附件地址
        /// </summary>
        public static string AttmntUrl
        {
            get
            {
                if (Global.AttmntServer==null)
                {
                    return "";
                }
                return Global.AttmntServer.ServerDomain;
            }
        }
        public static AttmntServer AttmntServer
        {
            get;
            set;
        }
        public static APPInfo APPInfo
        {
            get;
            set;
        }
        internal static void InitSettings()
        {
            AttmntServer = DBBaseFactory.DALBase.QueryForObject<AttmntServer>("FrontApi_GetAttmntServer", null);
            APPInfo = DBBaseFactory.DALBase.QueryForObject<APPInfo>("HTSmart_GetAPPInfo", null);
        }


    }


}
