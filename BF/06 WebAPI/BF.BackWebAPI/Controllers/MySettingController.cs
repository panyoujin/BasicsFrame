using BF.BackWebAPI.Models.Front;
using BF.BackWebAPI.Models.Front.Request;
using BF.BackWebAPI.Models.Front.Response;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class MySettingController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage MyShop(string Name = "", string Description = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var startSize = this.GetStartSize(page, pageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("PageSize", pageSize);
            dic.Add("Name", Name);
            dic.Add("Description", Description);
            List<MyShopResponse> shops = DBBaseFactory.DALBase.QueryForList<MyShopResponse>("Get_MyShoppings", dic);
            if (shops != null && shops.Count > 0)
            {
                foreach (var item in shops)
                {
                    if (!string.IsNullOrEmpty(item.Url))
                    {
                        item.Url = string.IsNullOrEmpty(item.Url) ? "" : Global.AttmntUrl + item.Url;
                    }
                }
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForList<MyShopResponse>("Get_MyShoppings", dic);

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="version">当前app版本</param>
        /// <param name="appType">0:android  1:ios</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAppVersion(decimal version, int appType = 0)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Version", version + "");
            dic.Add("AppType", appType + "");

            AppVersionResponse app = DBBaseFactory.DALBase.QueryForObject<AppVersionResponse>("Get_AppVersion", dic);
            if (app != null)
            {
                if (!string.IsNullOrEmpty(app.TargetUrl))
                {
                    app.TargetUrl = Global.AttmntUrl + app.TargetUrl;
                }
            }
            apiResult.data = app;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
