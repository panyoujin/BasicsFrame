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
    public class Back_AppVersionController : BaseController
    {

        public HttpResponseMessage GetAppVersions(int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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


            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("BackWeb_GetAppVersion", dic);

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
        public HttpResponseMessage InsertAppVersion(string text_Version, int text_AppType, string text_Target_Url, int text_Update_Status, string text_VersionDate, int text_FileSize, string text_UpdateContent)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Version", text_Version);
            dic.Add("Update_Status", text_Update_Status);
            dic.Add("Target_Url", text_Target_Url);
            dic.Add("AppType", text_AppType);
            dic.Add("VersionDate", text_VersionDate);
            dic.Add("FileSize", text_FileSize);
            dic.Add("CreationUser", MemberInfo.Account);
            dic.Add("UpdateContent", text_UpdateContent);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_InsertAppVersion", dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
                apiResult.msg = ResultMsg.CODE_EXCEPTION;
            }


            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
