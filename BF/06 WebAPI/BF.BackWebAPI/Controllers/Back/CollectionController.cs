using BF.BackWebAPI.Authorize;
using BF.BackWebAPI.Models.Back.InParam;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class CollectionController : BaseController
    {
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="source_ID"></param>
        /// <param name="source_Type">收藏源类型 1.模式;2.文章;</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddCollection([FromBody]Collection model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", model.source_ID);
            dic.Add("Source_Type", model.source_Type);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddCollection", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="source_ID"></param>
        /// <param name="source_Type">收藏源类型 1.模式;2.文章;</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelCollection([FromBody]Collection model)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", model.source_ID);
            dic.Add("Source_Type", model.source_Type);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            dic.Add("User_ID", this.MemberInfo.ID);
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_CancelCollection", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 根据类型获取收藏列表
        /// </summary>
        /// <param name="type">收藏源类型 1.模式;2.文章;</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// 暂时只支持模式
        [HttpGet]
        [BFAuthorizeAttribute(IsLogin = true)]
        public HttpResponseMessage GetCollectionListByType(int type = 0, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            dic.Add("User_ID", this.MemberInfo.ID);
            if (type>0)
            {
                dic.Add("Source_Tye", type);
            }
            apiResult.data = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetCollectionListByType", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}