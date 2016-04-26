﻿using BF.Common.Helper;
using BF.Common.SQLAnalytical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BF.BackWebAPI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CacheSqlConfig.Instance.SqlConfigPath = Server.MapPath("/bin/SqlConfig");
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
            string errMsg = string.Format("请求URL：{0},用户名:{1},请求类型:{2}{3}", url, "", Request.RequestType, postParameter);
            LogHelper.Info(errMsg);
        }

    }
}
