using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        [Authorize(Roles ="Users")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Users")]
        public string GetMemnuList(string name="")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MenuName", string.Format("%{0}%", name));
            var dt = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetMenuList", dic);
            ApiResult<object> api = new ApiResult<object>();
            api.code = "200";
            api.msg = "请求成功";
            api.data = dt;
            return JsonHelper.SerializeObject(api);
        }
    }
}