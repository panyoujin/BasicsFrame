using BF.BackWebAPI.Models.Back.OutParam;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Encrypt;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class Back_MemberController : ApiController
    {
        public HttpResponseMessage GetMemberInfos(string search = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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

            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_MemberInfoList", dic);

            if (ds != null && ds.Tables.Count >= 2)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (item["ImageUrl"] != null && !string.IsNullOrWhiteSpace(item["ImageUrl"].ToString()))
                    {
                        item["ImageUrl"] = Global.AttmntUrl + item["ImageUrl"].ToString();
                    }
                }
                var result = new { table = DBBaseFactory.DALBase.TableToList<MemberInfo>(ds.Tables[0]), total = ds.Tables[1].Rows[0][0] };
                apiResult.data = result;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        public HttpResponseMessage GetDevices(string search = "",int memberID=0, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            if (memberID > 0)
            {
                dic.Add("memberID", memberID);
            }
            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_DeviceList", dic);

            if (ds != null && ds.Tables.Count >= 2)
            {

                var result = new { table = DBBaseFactory.DALBase.TableToList<DeviceInfo>(ds.Tables[0]), total = ds.Tables[1].Rows[0][0] };
                apiResult.data = result;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ResetPasswd(int memberID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("memberID", memberID);
            dic.Add("NewPasswd", MD5Encrypt.Md5("a123456"));

            int result = DBBaseFactory.DALBase.ExecuteNonQuery("Back_ResetPasswd", dic);
            if (result > 0)
            {
                apiResult.data = true;
            }
            else
            {
                apiResult.code = ResultCode.CODE_EXCEPTION;
                apiResult.msg = "修改密码失败！";
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
