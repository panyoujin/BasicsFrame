using BF.BackWebAPI.Models.Back.InParam;
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
    public class Back_ModelTypeController : BaseController
    {
        #region --ModelType--
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="type_name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public HttpResponseMessage GetModelTypes(string type_name = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_GetModelTypeList", dic);

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
        public HttpResponseMessage InsertModelType([FromBody]InsertModelType param)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_InsertModelType";
            if (param.ID > 0)
            {
                key = "Back_UpdateModelType";
                dic.Add("ModificationUser", "admin");
                dic.Add("ID", param.ID + "");
            }
            else
            {
                dic.Add("CreationUser", "admin");
            }
            dic.Add("Name", param.Name);
            dic.Add("TypeDescribe", param.TypeDescribe);
            dic.Add("ImageUrl", param.ImageUrl);
            dic.Add("TypeSort", param.TypeSort + "");

            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage QueryModelTypeByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_QueryModelTypeByID";
            dic.Add("ID", ID + "");
            var result = DBBaseFactory.DALBase.QueryForObject<ModelType>(key, dic);
            if (result != null && !string.IsNullOrWhiteSpace(result.ImageUrl))
            {
                result.FullUrl = Global.AttmntUrl + result.ImageUrl;
            }
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        
        [HttpGet]
        public HttpResponseMessage DeleteModelTypeByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_DeleteModelTypeByID";
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
