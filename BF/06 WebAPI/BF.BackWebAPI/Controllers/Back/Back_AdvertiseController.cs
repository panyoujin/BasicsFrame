﻿using BF.BackWebAPI.Models.Back.InParam;
using BF.BackWebAPI.Models.Back.OutParam;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.FileProcess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class Back_AdvertiseController : ApiController
    {
        #region --AdvertiseType--
        /// <summary>
        /// 广告分类列表
        /// </summary>
        /// <param name="type_name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAdvertiseTypes(string Name = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            if (!string.IsNullOrWhiteSpace(Name))
            {
                dic.Add("Name", "%" + Name + "%");
            }
            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_GetAdvertiseTypeList", dic);

            if (ds != null && ds.Tables.Count >= 2)
            {
                var result = new { table = ds.Tables[0], total = ds.Tables[1].Rows[0][0] };
                apiResult.data = result;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpPost]
        public HttpResponseMessage InsertAdvertiseType([FromBody]InsertAdvertiseType param)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_InsertAdvertiseType";
            if (param.ID > 0)
            {
                key = "Back_UpdateAdvertiseType";
                dic.Add("ModificationUser", "admin");
                dic.Add("ID", param.ID + "");
            }
            else
            {
                dic.Add("CreationUser", "admin");
            }
            dic.Add("Name", param.Name);
            dic.Add("Description", param.Description);
            dic.Add("Code", param.Code);

            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage QueryAdvertiseTypeByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_QueryAdvertiseTypeByID";
            dic.Add("ID", ID + "");
            var result = DBBaseFactory.DALBase.QueryForObject<AdvertiseType>(key, dic);
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        
        [HttpGet]
        public HttpResponseMessage DeleteAdvertiseTypeByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_DeleteAdvertiseTypeByID";
            dic.Add("ID", ID + "");
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        #endregion

        #region --- Article ---
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="type_name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public HttpResponseMessage GetArticles(string search = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            if (!string.IsNullOrWhiteSpace(search))
            {
                dic.Add("Search", "%" + search + "%");
            }

            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_GetArticleList", dic);

            if (ds != null && ds.Tables.Count >= 2)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (item["ImageUrl"] != null && !string.IsNullOrWhiteSpace(item["ImageUrl"].ToString()))
                    {
                        item["FullUrl"] = Global.AttmntUrl + item["ImageUrl"].ToString();
                    }
                }
                var result = new { table = ds.Tables[0], total = ds.Tables[1].Rows[0][0] };
                apiResult.data = result;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpPost]
        public HttpResponseMessage InsertArticle([FromBody]InsertArticle param)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_InsertArticle";
            if (param.ID > 0)
            {
                key = "Back_UpdateArticle";
                dic.Add("ModificationUser", "admin");
                dic.Add("ID", param.ID + "");
            }
            else
            {
                dic.Add("CreationUser", "admin");
            }
            dic.Add("ArticleType_ID", param.ArticleType_ID+"");
            dic.Add("ArticleTitle", param.ArticleTitle);
            dic.Add("ArticleContent", param.ArticleContent);
            dic.Add("ImageUrl", param.ImageUrl);
            dic.Add("PublishDate", param.PublishDate + "");
            dic.Add("ArticleSort", param.ArticleSort + "");
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage QueryArticleByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_QueryArticleByID";
            dic.Add("ID", ID + "");
            var result = DBBaseFactory.DALBase.QueryForObject<InsertArticle>(key, dic);
            if (result != null && !string.IsNullOrEmpty(result.ImageUrl))
            {
                result.FullUrl = Global.AttmntUrl + result.ImageUrl;
            }
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpGet]
        public HttpResponseMessage DeleteArticleByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_DeleteArticleByID";
            dic.Add("ID", ID + "");
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        #endregion

    }
}
