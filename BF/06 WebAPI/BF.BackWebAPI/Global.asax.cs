using BF.Common.Helper;
using BF.Common.SQLAnalytical;
using System;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
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
            string errMsg = string.Format("请求URL：{0},IP:{1},请求类型:{2}{3}", url, RequestHelper.GetIP(), Request.RequestType, postParameter);
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
}
