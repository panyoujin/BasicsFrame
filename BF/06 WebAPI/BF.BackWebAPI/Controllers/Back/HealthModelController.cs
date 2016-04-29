using BF.BackWebAPI.Authorize;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers.Back
{
    public class HealthModelController : BaseController
    {

        /// <summary>
        /// 根据类型获取模式列表
        /// </summary>
        /// <param name="type">类型：0.系统提供;1.自定义</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [BFAuthorizeAttribute(IsLogin =false)]
        public string GetHealthModelList(int type = 0, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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

        /// <summary>
        /// 根据模式ID 获取模式，需要检验当前用户是否有权限访问该模式
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHealthModelInfo(int modelID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ModelID", modelID);
            if (!UserInfo.IsAdmin)
            {
                dic.Add("User_ID", UserInfo.ID);
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetHealthModelInfoByModelID", dic);
            return JsonHelper.SerializeObject(apiResult);
        }
    }
}