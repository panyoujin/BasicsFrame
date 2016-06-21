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
    public class Back_FeedbackController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage GetFeedbackList(int status = -1, string search = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            var startSize = 0;
            var endSize = 0;
            this.SetPageSize(page, pageSize, ref startSize, ref endSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("PageSize", pageSize);
            if (status != -1)
            {
                dic.Add("FeedbackStatus", status);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                dic.Add("search", "%" + search + "%");
            }
            var ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetFeedbackList", dic);
            int total = 0;
            int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out total);
            apiResult.data = new { dataList = ds.Tables[0], total = total };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
