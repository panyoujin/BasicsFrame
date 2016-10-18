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
    public class Back_ShareController : ApiController
    {

        public HttpResponseMessage GetUserShares(string search = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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

            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetAllUserShare", dic);

            if (ds != null && ds.Tables.Count >= 2)
            {
                //foreach (DataRow item in ds.Tables[0].Rows)
                //{
                //    if (item["ImageUrl"] != null && !string.IsNullOrWhiteSpace(item["ImageUrl"].ToString()))
                //    {
                //        item["FullUrl"] = Global.AttmntUrl + item["ImageUrl"].ToString();
                //    }
                //}
                var result = new { table = ds.Tables[0], total = ds.Tables[1].Rows[0][0] };
                apiResult.data = result;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage SetHot(int id, int hot)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Hot", hot);
            dic.Add("ID", id);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_SetShareHot", dic);
            if (result <= 0) {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
                apiResult.msg = ResultMsg.CODE_EXCEPTION;
            }
            

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage SetHotSort(int id, int sort)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("HotSort", sort);
            dic.Add("ID", id);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_SetShareHotSort", dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
                apiResult.msg = ResultMsg.CODE_EXCEPTION;
            }


            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
