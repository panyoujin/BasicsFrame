using BF.BackWebAPI.Models.Request;
using BF.BackWebAPI.Models.Response;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers
{
    public class ShareController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxID">已经获取的最大ID</param>
        /// <param name="minID">已经获取的最小ID</param>
        /// <param name="request_type">获取类型：
        /// new：获取当前没获取过的新动态
        ///  next：获取当前获取后的下一页
        /// </param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShareListByID(int maxID, int minID, string request_type = "next", int pageSize = CommonConstant.PAGE_SIZE)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("User_ID", this.MemberInfo.ID);
            if (request_type.ToLower() == "new")
            {
                if (maxID > 0)
                {
                    dic.Add("MaxID", maxID);
                }
            }
            else
            {
                if (pageSize <= 0)
                {
                    pageSize = 10;
                }
                if (minID > 0)
                {
                    dic.Add("MinID", minID);
                }
                dic.Add("StartSize", 0);
                dic.Add("PageSize", pageSize);
            }
            var spList = DBBaseFactory.DALBase.QueryForList<ShareResponse>("BackWeb_GetShareListByID", dic) ?? new List<ShareResponse>();
            if (spList != null && spList.Count > 0)
            {
                var tempID = spList.Max(s => s.ID);
                maxID = maxID > tempID ? maxID : tempID;

                tempID = spList.Min(s => s.ID);
                minID = minID > 0 && minID < tempID ? minID : tempID;
            }

            apiResult.data = new { ShareList = spList, MaxID = maxID, MinID = minID };
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        [HttpPost]
        public HttpResponseMessage AddShare([FromBody]AddShareRequest share)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Source_ID", share.Source_ID);
            dic.Add("Source_Type", share.Source_Type);
            dic.Add("ShareTitle", share.ShareTitle);
            dic.Add("ShareContent", share.ShareContent);
            dic.Add("ShareUrl", share.ShareUrl);
            dic.Add("User_ID", this.MemberInfo.ID);
            dic.Add("UserAccount", this.MemberInfo.Account ?? this.MemberInfo.ID + "");
            DBBaseFactory.DALBase.ExecuteNonQuery("BackWeb_AddShare", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
    }
}