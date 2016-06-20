using BF.BackWebAPI.Models.ResponseModel;
using BF.Common.CommonEntities;
using BF.Common.CustomException;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class ArticController : BaseController
    {
        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetArticleTypeList()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            var dt = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetArticleTypeList", null);
            apiResult.data = dt;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetArticleListByTypeID(int typeID = -1)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (typeID > 0)
            {
                dic.Add("ArticleType_ID", typeID);
            }
            var articleList = DBBaseFactory.DALBase.QueryForList<ArticleListResponse>("BackWeb_GetArticleListByTypeID", dic);
            apiResult.data = articleList;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetArticleInfoByID(int articleID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            if (articleID <= 0)
            {
                throw new BusinessException("文章不存在");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Article_ID", articleID);
            try
            {
                dic.Add("User_ID", this.MemberInfo.ID);
            }
            catch (Exception ex)
            {

            }
            var obj = DBBaseFactory.DALBase.QueryForObject<ArticleInfoResponse>("BackWeb_GetArticleInfoByID", dic);
            apiResult.data = obj;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}