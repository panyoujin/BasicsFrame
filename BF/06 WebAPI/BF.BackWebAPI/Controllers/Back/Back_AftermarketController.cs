using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.RequestModels;
using BF.BackWebAPI.Models.ResponseModel;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Enums;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class Back_AftermarketController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage GetAftermarketList(int status, string code, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            var m = this.MemberInfo;
            var startSize = this.GetStartSize(page, pageSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("PageSize", pageSize);
            if (status != -1)
            {
                dic.Add("AftermarketStatus", status);
            }
            if (!string.IsNullOrWhiteSpace(code))
            {
                dic.Add("ProductCode", "%" + code + "%");
            }
            var ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetAftermarketList", dic);
            int total = 0;
            int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out total);
            apiResult.data = new { dataList = DBBaseFactory.DALBase.TableToList<AftermarketResponse>(ds.Tables[0]), total = total };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpPost]
        public HttpResponseMessage SetAftermarketStatus()
        {
            int aid = 0;
            int.TryParse(HttpContext.Current.Request.Form["aid"], out aid);
            int status = 0;
            int.TryParse(HttpContext.Current.Request.Form["status"], out status);
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("AID", aid);
            dic.Add("AftermarketStatus", status);
            dic.Add("UserAccount", this.MemberInfo.Account);
            var count = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_SetAftermarketStatus", dic);
            apiResult.data = count;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
