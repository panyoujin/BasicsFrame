using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class ArticleController : ApiController
    {
        public HttpResponseMessage GetArticleTypes(string type_name = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            var startSize = (page > 1 ? (page - 1) * pageSize : 0);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("StartSize", startSize);
            dic.Add("PageSize", pageSize);
            if (!string.IsNullOrWhiteSpace(type_name))
            {
                dic.Add("Type_Name", "%" + type_name + "%");
            }
            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_GetArticleTypeList", dic);
            apiResult.data = ds.Tables[0];

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
