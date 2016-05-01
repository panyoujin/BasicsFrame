using BF.BackWebAPI.Models.Front;
using BF.BackWebAPI.Models.Front.Request;
using BF.BackWebAPI.Models.Front.Response;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Front
{
    public class MySettingController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage MyShop(string Name = "", string Description = "", int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE)
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
            dic.Add("Name", Name);
            dic.Add("Description", Description);

            apiResult.data = DBBaseFactory.DALBase.QueryForList<MyShopResponse>("Get_MyShoppings", dic);

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpPost]
        public HttpResponseMessage NewKettle([FromBody]KettleModel param)
        {
            ApiResult<bool> apiResult = new ApiResult<bool>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Name", param.Name);
            dic.Add("Version", param.Version);
            dic.Add("Description", param.Description);
            dic.Add("CreationUser", MemberInfo.Account);
            //
            //通过接口检查水壶是否符合产品标准
            //
            var kettles = DBBaseFactory.DALBase.QueryForObject<KettleModel>("Get_CheckKettle", dic);
            if (kettles == null)
            {
                int result = DBBaseFactory.DALBase.ExecuteNonQuery("Insert_Kettle", dic);
                kettles = DBBaseFactory.DALBase.QueryForObject<KettleModel>("Get_CheckKettle", dic);
            }
            if (kettles != null)
            {
                dic.Add("MemberID", MemberInfo.ID);
                dic.Add("KettleID", kettles.ID);
                var kettleRel = DBBaseFactory.DALBase.QueryForObject<MyKettleRelResponse>("Get_CheckMyKettleRel", dic);
                if (kettleRel == null)
                {

                    dic.Add("Default", false);
                    DBBaseFactory.DALBase.ExecuteNonQuery("Insert_MyKettleRel", dic);
                }
                else
                {
                    apiResult.msg = "水壶已经绑定过！";
                }
            }
            apiResult.data = true;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpGet]
        public HttpResponseMessage QueryMyKettle()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MemberID", MemberInfo.ID);

            apiResult.data = DBBaseFactory.DALBase.QueryForList<KettleModel>("Get_QueryMyKettles", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}
