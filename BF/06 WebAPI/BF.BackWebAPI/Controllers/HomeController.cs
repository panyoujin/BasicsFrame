using BF.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", 1);
            dic.Add("MenuName", "%根节点%");
            var dt = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetMenuList", dic);
            return View();
        }
        
    }
}