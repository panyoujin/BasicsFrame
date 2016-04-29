using BF.BackWebAPI.Authorize;
using System.Web.Http;

namespace BF.BackWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "znsh/{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "login", id = RouteParameter.Optional }
            );
            config.Filters.Add(new ExceptionAttribute());
        }
    }
}
