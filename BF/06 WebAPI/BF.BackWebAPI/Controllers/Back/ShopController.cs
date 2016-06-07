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
    public class ShopController : ApiController
    {
        public HttpResponseMessage GetShops(string search = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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

            DataSet ds = DBBaseFactory.DALBase.QueryForDataSet("Back_GetShopsList", dic);

            if (ds != null && ds.Tables.Count >= 2)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (item["ImageUrl"] != null && !string.IsNullOrWhiteSpace(item["ImageUrl"].ToString()))
                    {
                        item["ImageUrl"] = Global.AttmntUrl + item["ImageUrl"].ToString();
                    }
                }
                var result = new { table = ds.Tables[0], total = ds.Tables[1].Rows[0][0] };
                apiResult.data = result;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpPost]
        public HttpResponseMessage InsertShop([FromBody]InsertShop param)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_InsertShopping";
            if (param.ID > 0)
            {
                key = "Back_UpdateShopping";
                dic.Add("ModificationUser", "admin");
                dic.Add("ID", param.ID + "");
            }
            else
            {
                dic.Add("CreationUser", "admin");
            }
            dic.Add("Name", param.Name);
            dic.Add("ImageUrl", param.ImageUrl);
            dic.Add("ContentUrl", param.ContentUrl);
            dic.Add("Price", param.Price);
            dic.Add("NowPrice", param.NowPrice);
            dic.Add("Description", param.Description);
            
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
            {
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            }

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage QueryShopByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_QueryShopping";
            dic.Add("ID", ID);
            var result = DBBaseFactory.DALBase.QueryForObject<InsertShop>(key, dic);
            if (result != null && !string.IsNullOrEmpty(result.ImageUrl))
            {
                result.FullUrl = Global.AttmntUrl + result.ImageUrl;
            }
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpGet]
        public HttpResponseMessage DeleteShopByID(int ID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_DeleteShopping";
            dic.Add("ID", ID);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpGet]
        public HttpResponseMessage EnableShopByID(int ID, int Enable)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_EnableShopping";
            dic.Add("ID", ID);
            dic.Add("Enable", Enable);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpGet]
        public HttpResponseMessage OnShelfShopByID(int ID, int OnShelf)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string key = "Back_OnShelfShopping";
            dic.Add("ID", ID);
            dic.Add("OnShelf", OnShelf);
            int result = DBBaseFactory.DALBase.ExecuteNonQuery(key, dic);
            if (result <= 0)
                apiResult.code = ResultCode.CODE_UPDATE_FAIL;
            apiResult.data = result;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
