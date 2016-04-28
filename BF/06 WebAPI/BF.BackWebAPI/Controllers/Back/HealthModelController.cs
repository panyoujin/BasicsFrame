using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers.Back
{
    public class HealthModelController : BackBaseController
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">类型：0.系统提供;1.自定义</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHealthModelList(int type=0, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            dic.Add("Model_Type", type);

            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetHealthModelListByType", dic);
            return JsonHelper.SerializeObject(apiResult);
        }
    }
}